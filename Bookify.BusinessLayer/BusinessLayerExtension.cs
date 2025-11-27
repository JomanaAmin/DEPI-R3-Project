using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer
{
    public static class BusinessLayerExtension
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection service) 
        {
            service.AddScoped<IRoomService,RoomService>();
            service.AddScoped<IRoomTypeService, RoomTypeService>();
            service.AddScoped<ICartService, CartService>();
            service.AddScoped<IPaymentService, PaymentService>();
            service.AddScoped<ICustomerProfileService, CustomerProfileService>();
            service.AddScoped<IBookingService, BookingService>();
            service.AddScoped<IJwtService, JwtService>();
            service.AddScoped<IAdminProfileService, AdminProfileService>();
            service.AddScoped<IImageStorageService,ImageStorageService>();
            
            return service;
        }
    }
}
