using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Bookify.DAL.Contexts; 
using Bookify.DAL.Entities;

internal class Program
{
    static void Main(string[] args)
    {
        // Load the connection string from appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        // Configure DbContext
        var optionsBuilder = new DbContextOptionsBuilder<BookifyDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

       
        
    }
}
