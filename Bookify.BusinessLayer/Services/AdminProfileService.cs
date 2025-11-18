using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs.AdminProfileDTOs;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs.CustomerProfileDTOs;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;
using Bookify.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
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
        public Task<CustomerProfileViewDTO> GetAdminProfileAsync(string adminId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAdminDetailsAsync(string adminId, AdminProfileUpdateDTO updateDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAdminSProfileAsync(string customerId)
        {
            throw new NotImplementedException();
        }
    }
}
