using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.CustomExceptions;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs.AdminProfileDTOs;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs.CustomerProfileDTOs;
using Bookify.BusinessLayer.DTOs.BookingDTOs;
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
    internal class CustomerProfileService : BaseProfileService, ICustomerProfileService
    {
        private IGenericRepository<CustomerProfile> customerProfileRepository;

        public CustomerProfileService(IUnitOfWork unitOfWork, UserManager<BaseUser> userManager) : base(unitOfWork, userManager) 
        {
            this.customerProfileRepository = unitOfWork.CustomerProfiles;
        }

        //////CREATE CUSTOMER PROFILE//////
        protected override async Task<BaseUser> CreateSpecificProfileAndSaveAsync(BaseUser user,BaseUserCreateDTO baseUserCreateDTO)
        {

            var customerProfile = new CustomerProfile
            {
                CustomerId = user.Id, // Links to BaseUser.Id
                FirstName = baseUserCreateDTO.FirstName ?? "Guest",
                LastName = baseUserCreateDTO.LastName ?? "User",
                Cart = new DAL.Entities.Cart
                {
                    CustomerId = user.Id
                }
            };

            await customerProfileRepository.CreateAsync(customerProfile);
            await unitOfWork.SaveChangesAsync();
            return user;
        }
        ///////Register entry point////////
        public async Task RegisterCustomerAsync(BaseUserCreateDTO baseUserCreateDTO)
        {
            bool emailExists=await EmailExistsAsync(baseUserCreateDTO.Email);
            if (emailExists)
            {
                throw new EmailInvalidException("Email already in use.");
            }
            BaseUser user = new BaseUser
            {
                UserName = baseUserCreateDTO.Email,
                Email = baseUserCreateDTO.Email
            };
            var result = await userManager.CreateAsync(user,baseUserCreateDTO.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            await FinalizeRegistrationAsync(user,baseUserCreateDTO);
        }

        protected override string GetRoleName()
        {
            return "Customer";
        }
        public async Task<CustomerProfileViewDTO> ChangePassword(BaseUserChangePasswordDTO baseUserChangePasswordDTO)
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
            var result = await ChangePasswordAsync(user, baseUserChangePasswordDTO.CurrentPassword, baseUserChangePasswordDTO.NewPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to change password");
            }
            return await GetCustomerProfileAsync(user.Id);
        }

        public async Task<CustomerProfileViewDTO> GetCustomerProfileAsync(string customerId)
        {
            CustomerProfileViewDTO? customerProfileViewDTO = await customerProfileRepository.GetAllAsQueryable().AsNoTracking().Where(cp => cp.CustomerId == customerId)
                .Select(cp => new CustomerProfileViewDTO
                {
                    CustomerId = cp.CustomerId,
                    Email = cp.User.Email,
                    FullName = cp.FirstName + " " + cp.LastName,
                    CreatedAt = cp.CreatedAt
                }

                )
                .SingleOrDefaultAsync();
            if (customerProfileViewDTO == null) 
            {
                throw new Exception("Customer profile not found.");
            }
            return customerProfileViewDTO;
        }

        public async Task<CustomerProfileViewDTO> UpdateCustomerDetailsAsync(string customerId, CustomerProfileUpdateDTO updateDto)
        {
            CustomerProfile? customerProfile = await customerProfileRepository.GetByIdAsync(customerId);
            if (customerProfile == null)
            {
                throw new Exception("Customer profile not found.");
            }
            customerProfile.FirstName = updateDto.FirstName;
            customerProfile.LastName = updateDto.LastName;
            customerProfileRepository.Update(customerProfile);
            await unitOfWork.SaveChangesAsync();
            return await GetCustomerProfileAsync(customerId);
        }

        public async Task DeleteCustomerProfileAsync(string customerId)
        {
            CustomerProfile? customerProfile = await customerProfileRepository.GetAllAsQueryable().AsNoTracking().Where(cp => cp.CustomerId == customerId).Include(cp => cp.User).SingleOrDefaultAsync();
            if (customerProfile == null)
            {
                throw new Exception("Customer profile not found.");
            }
            
            CustomerProfile? toBeDeletedResult = await customerProfileRepository.Delete(customerId);
            if (toBeDeletedResult == null) throw new Exception("Customer profile not found");
            var success=await DeleteByUserAsync(customerProfile.User);
            if (!success) 
            {
                throw new Exception("Could not delete associated user account.");
            }
            await unitOfWork.SaveChangesAsync();
        }

        public Task<List<BookingViewDTO>> ViewCustomerBookingsAsync(string customerId)
        {
            throw new NotImplementedException();
        }
    }
}
