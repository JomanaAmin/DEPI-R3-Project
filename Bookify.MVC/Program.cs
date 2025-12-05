using Bookify.MVC.Contracts;
using Bookify.MVC.Services;
using Microsoft.Extensions.FileProviders;

namespace Bookify.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<JwtHandler>();
            builder.Services.AddScoped<CartMVCService>();
            builder.Services.AddScoped<RoomTypeService>();
            // Configure logging
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            
            // Configure HttpClient for RoomServices with proper error handling
            builder.Services.AddHttpClient<IRoomServices, RoomServices>(c =>
            {
                var baseAddress = builder.Configuration["ApiBaseAddress:BaseURL"];
                Console.WriteLine($"Configuring RoomServices with BaseAddress: {baseAddress}");
                c.BaseAddress = new Uri(baseAddress);
                c.Timeout = TimeSpan.FromSeconds(30); // Add timeout
            }).AddHttpMessageHandler<JwtHandler>();



            builder.Services.AddHttpClient<RoomTypeService>(
                c=> { c.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress:BaseURL"]); }).AddHttpMessageHandler<JwtHandler>();

            builder.Services.AddHttpClient<AccountMVCService>(
                c=> { c.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress:BaseURL"]); }).AddHttpMessageHandler<JwtHandler>();
            builder.Services.AddHttpClient<AdminService>(
                c=> { c.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress:BaseURL"]); }).AddHttpMessageHandler<JwtHandler>();

            builder.Services.AddAuthentication("Cookies")
            .AddCookie("Cookies", opts =>
            {
                opts.LoginPath = "/Account/Login";
            });
            builder.Services.AddDistributedMemoryCache(); // required for session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1); // how long the session lasts
                options.Cookie.HttpOnly = true;             // security
                options.Cookie.IsEssential = true;          // required for GDPR
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var frontendBase = Path.Combine(builder.Environment.ContentRootPath, "frontend_temp");
            if (!Directory.Exists(frontendBase))
            {
                frontendBase = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "frontend_temp"));
            }

            var staticRoot = frontendBase;
            if (Directory.Exists(frontendBase))
            {
                var distPath = Path.Combine(frontendBase, "dist");
                var buildPath = Path.Combine(frontendBase, "build");
                if (File.Exists(Path.Combine(distPath, "index.html"))) staticRoot = distPath;
                else if (File.Exists(Path.Combine(buildPath, "index.html"))) staticRoot = buildPath;

                app.Use(async (context, next) =>
                {
                    var path = context.Request.Path.Value ?? string.Empty;
                    if (path.Equals("/app/room.html", StringComparison.OrdinalIgnoreCase))
                    {
                        var id = context.Request.Query["id"].ToString();
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            context.Response.Redirect($"/RoomDetails?id={id}", permanent: false);
                            return;
                        }
                        else
                        {
                            // No id, go to rooms list
                            context.Response.Redirect("/Rooms", permanent: false);
                            return;
                        }
                    }
                    if (path.EndsWith(".html") && !path.StartsWith("/app/"))
                    {
                        var fileCandidate = Path.Combine(staticRoot, path.TrimStart('/'));
                        if (File.Exists(fileCandidate))
                        {
                            context.Response.Redirect("/app" + path, permanent: false);
                            return;
                        }
                    }
                    await next();
                });

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(staticRoot),
                    RequestPath = "/app"
                });

                var libPath = Path.Combine(staticRoot, "lib");
                if (Directory.Exists(libPath))
                {
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(libPath),
                        RequestPath = "/lib"
                    });
                }

                string[] rootFolders = { "css", "js", "img", "images", "assets" };
                foreach (var folder in rootFolders)
                {
                    var path = Path.Combine(staticRoot, folder);
                    if (Directory.Exists(path))
                    {
                        app.UseStaticFiles(new StaticFileOptions
                        {
                            FileProvider = new PhysicalFileProvider(path),
                            RequestPath = $"/{folder}"
                        });
                    }
                }

                var indexFile = Path.Combine(staticRoot, "index.html");
                if (File.Exists(indexFile))
                {
                    app.MapGet("/", async context =>
                    {
                        context.Response.ContentType = "text/html";
                        await context.Response.SendFileAsync(indexFile);
                    });

                    app.MapFallback("/app/{*path}", async context =>
                    {
                        context.Response.ContentType = "text/html";
                        await context.Response.SendFileAsync(indexFile);
                    });
                }
            }
            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();  
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
