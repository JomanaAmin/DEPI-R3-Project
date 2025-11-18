using Bookify.BusinessLayer.Contracts;
using Bookify.DAL.Entities;
using Bookify.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Services
{
    internal class CustomerProfileService : BaseProfileService, ICustomerProfileService
    {
        public CustomerProfileService(IUnitOfWork unitOfWork, UserManager<BaseUser> userManager) : base(unitOfWork, userManager) { }
        protected override async Task<string> CreateSpecificProfileAndSaveAsync(BaseUser baseUser)
        {
            string customerId = baseUser.Id;

            // Create the linked AdminProfile entity
            var customerProfile = new CustomerProfile
            {
                CustomerId = customerId, // Links to BaseUser.Id
                FirstName = baseUser.CustomerProfile?.FirstName ?? "Guest",
                LastName = baseUser.CustomerProfile?.LastName ?? "User",
            };

            await unitOfWork.CustomerProfiles.CreateAsync(customerProfile);
            await unitOfWork.SaveChangesAsync();

            return customerId;
        }

        protected override string GetRoleName()
        {
            return "Customer";
        }
    }
}
