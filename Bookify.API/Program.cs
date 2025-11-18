using Bookify;
using Bookify.BusinessLayer;
using Bookify.DAL.Contexts;
using Bookify.DAL.Entities;
using Microsoft.AspNetCore.Identity;
namespace Bookify.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddBusinessLayer();
            builder.Services.AddDataAccessLayer(builder.Configuration);
            builder.Services.AddIdentity<BaseUser, IdentityRole>()
            .AddEntityFrameworkStores<BookifyDbContext>()
            .AddDefaultTokenProviders();

            //custom identity options for testing purposes
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // WARNING: These settings are too weak for production. Use for testing only.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
                options.SignIn.RequireConfirmedEmail = false;
            });
            var app = builder.Build();

            //app.MapIdentityApi< IdentityUser >();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
