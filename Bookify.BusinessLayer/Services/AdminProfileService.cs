using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs.AdminProfileDTOs;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;
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
    internal class AdminProfileService : BaseProfileService,IAdminProfileService
    {
        private IGenericRepository<AdminProfile> adminProfileRepository;
        public AdminProfileService(IUnitOfWork unitOfWork, UserManager<BaseUser> userManager) : base(unitOfWork, userManager) 
        {
            this.adminProfileRepository = unitOfWork.AdminProfiles;
        }
        protected override async Task<BaseUser> CreateSpecificProfileAndSaveAsync(BaseUser user, BaseUserCreateDTO baseUserCreateDTO)
        {

            var adminProfile = new AdminProfile
            {
                AdminId = user.Id, // Links to BaseUser.Id
                FirstName = baseUserCreateDTO.FirstName ?? "System",
                LastName = baseUserCreateDTO.LastName ?? "Admin",
            };

            await adminProfileRepository.CreateAsync(adminProfile);
            await unitOfWork.SaveChangesAsync();
            return user;
        }
  
        protected override string GetRoleName()
        {
            return "Admin";
        }

        public async Task RegisterAdminAsync(BaseUserCreateDTO baseUserCreateDTO)
        {
            bool emailExists = await EmailExistsAsync(baseUserCreateDTO.Email);
            if (emailExists)
            {
                throw new Exception("Email already in use.");
            }
            BaseUser user = new BaseUser
            {
                UserName = baseUserCreateDTO.Email,
                Email = baseUserCreateDTO.Email
            };
            var result = await userManager.CreateAsync(user, baseUserCreateDTO.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            await FinalizeRegistrationAsync(user, baseUserCreateDTO);
        }

        public async Task<AdminProfileViewDTO> ChangePassword(BaseUserChangePasswordDTO baseUserChangePasswordDTO) 
        {
            BaseUser? user = await GetUserByEmailAsync(baseUserChangePasswordDTO.Email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            var passwordCheck = await userManager.CheckPasswordAsync(user, baseUserChangePasswordDTO.CurrentPassword);
            if (!passwordCheck)
            {
                throw new Exception("Current password is incorrect.");
            }
            if (baseUserChangePasswordDTO.NewPassword != baseUserChangePasswordDTO.ConfirmNewPassword)
            {
                throw new Exception("New password and confirmation do not match.");
            }
            var result = await ChangePasswordAsync(user, baseUserChangePasswordDTO.CurrentPassword,baseUserChangePasswordDTO.NewPassword);
            if (!result.Succeeded) 
            {
                throw new Exception("Failed to change password");
            }
            return await GetAdminProfileAsync(user.Id);
        }
        public async Task<AdminProfileViewDTO> GetAdminProfileAsync(string adminId)
        {
            AdminProfileViewDTO? adminProfileViewDTO = await adminProfileRepository.GetAllAsQueryable().AsNoTracking().Where(ap => ap.AdminId == adminId)
            .Select(ap => new AdminProfileViewDTO
            {
                AdminId = ap.AdminId,
                Email = ap.User.Email,
                FullName = ap.FirstName + " " + ap.LastName,
                CreatedAt = ap.CreatedAt
            }

            )
            .SingleOrDefaultAsync();
            if (adminProfileViewDTO == null)
            {
                throw new Exception("Admin profile not found.");
            }
            return adminProfileViewDTO;
        }

        public async Task<AdminProfileViewDTO> UpdateAdminDetailsAsync(string adminId, AdminProfileUpdateDTO updateDto)
        {
            AdminProfile? adminProfile = await adminProfileRepository.GetByIdAsync(adminId);
            if (adminProfile == null)
            {
                throw new Exception("admin profile not found.");
            }
            adminProfile.FirstName = updateDto.FirstName;
            adminProfile.LastName = updateDto.LastName;
            adminProfileRepository.Update(adminProfile);
            await unitOfWork.SaveChangesAsync();
            return await GetAdminProfileAsync(adminId);
        }

        public async Task DeleteAdminProfileAsync(string adminId)
        {
            AdminProfile? adminProfile = await adminProfileRepository.GetAllAsQueryable().AsNoTracking().Where(cp => cp.AdminId == adminId).Include(cp => cp.User).SingleOrDefaultAsync();
            if (adminProfile == null)
            {
                throw new Exception("admin profile not found.");
            }

            AdminProfile? toBeDeletedResult = await adminProfileRepository.Delete(adminId);
            if (toBeDeletedResult == null) throw new Exception("Admin profile not found");
            var success = await DeleteByUserAsync(adminProfile.User);
            if (!success)
            {
                throw new Exception("Could not delete associated user account.");
            }
            await unitOfWork.SaveChangesAsync();
        }
    }
}
