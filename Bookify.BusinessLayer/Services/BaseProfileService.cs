using Bookify.DAL.Contexts;
using Bookify.DAL.Entities;
using Bookify.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Services
{
    public abstract class BaseProfileService
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly UserManager<BaseUser> userManager;

        protected BaseProfileService(IUnitOfWork unitOfWork, UserManager<BaseUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }


        /// Handles the unique logic for creating the specific profile (Customer/Admin) and saving the changes to the database.
        /// <param name="baseUser">The newly created BaseUser entity.</param>
        /// <returns>The ID of the created user.</returns>
        protected abstract Task<string> CreateSpecificProfileAndSaveAsync(BaseUser baseUser);

        /// Gets the role to assign to the user (e.g., "Customer" or "Admin").
        protected abstract string GetRoleName();

        // --- Shared Core Registration Logic ---

        // This method executes the standard Identity steps
        public async Task<string> RegisterBaseUserAsync(BaseUser baseUser)
        {
            // 1. Assign the Role (Calling the abstract method to get the role name)
            await userManager.AddToRoleAsync(baseUser, GetRoleName());

            // 2. Execute the specific profile creation (delegated to the child class)
            string userId = await CreateSpecificProfileAndSaveAsync(baseUser);

            return userId;
        }
    }
}
