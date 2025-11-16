using Bookify.DAL.Contexts;
using Bookify.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify
{
    public static class DataAccessExtention
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection service, IConfigurationManager configurationManager)
        {
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddDbContext<BookifyDbContext>(options => { options.UseSqlServer(configurationManager.GetConnectionString("BookifyConnection")); });
            return service;
        }
    }
}
