using Bookify.BusinessLayer.Services;
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
            builder.Services.AddHttpClient<IRoomServices, RoomServices>(c =>
            {
                c.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress:BaseURL"]);
            });
            builder.Services.AddHttpClient<IAccountService, AccountService>(c =>
            {
                c.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress:BaseURL"]);
            });
            //builder.Services.AddScoped<IImageStorageService,LocalImageStorageService>();
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            //app.UseExceptionHandler();
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

                // make /app/page instead of /page only
                app.Use(async (context, next) =>
                {
                    var path = context.Request.Path.Value ?? string.Empty;
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
                    // opens frontend straight away
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

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
