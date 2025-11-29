using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
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
    public abstract class BaseProfileService : IBaseProfileService
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly UserManager<BaseUser> userManager;

        protected BaseProfileService(IUnitOfWork unitOfWork, UserManager<BaseUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        protected abstract Task<BaseUser> CreateSpecificProfileAndSaveAsync(BaseUser user, BaseUserCreateDTO baseUserCreateDTO);
        protected abstract string GetRoleName();

        //////CREATING A USER//////
        protected async Task<BaseUser> FinalizeRegistrationAsync(BaseUser baseUser, BaseUserCreateDTO baseUserCreateDTO )
        {           
            await userManager.AddToRoleAsync(baseUser, GetRoleName());
            await CreateSpecificProfileAndSaveAsync(baseUser,baseUserCreateDTO);
            return baseUser;
        }


        protected async Task<bool> EmailExistsAsync(string email)
        => await userManager.FindByEmailAsync(email) != null;

        protected async Task<BaseUser?> GetUserByIdAsync(string userId)
            => await userManager.FindByIdAsync(userId);
        protected async Task<bool> DeleteByUserAsync(BaseUser baseUser)
        {
            var res= await userManager.DeleteAsync(baseUser);
            return (res.Succeeded);
        }
        protected async Task<BaseUser?> GetUserByEmailAsync(string email)
            => await userManager.FindByEmailAsync(email);

        protected async Task<bool> CheckPasswordAsync(BaseUser user, string password)
            => await userManager.CheckPasswordAsync(user, password);

        protected async Task<IdentityResult> UpdateUserAsync(BaseUser user)
            => await userManager.UpdateAsync(user);

        protected async Task<IdentityResult> ChangePasswordAsync(
            BaseUser user, string oldPassword, string newPassword)
            => await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
    }
}
