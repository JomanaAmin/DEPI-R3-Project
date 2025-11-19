using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs.AdminProfileDTOs;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs.CustomerProfileDTOs;
using Bookify.BusinessLayer.DTOs.BookingDTOs;
using Bookify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Contracts
{
    public interface ICustomerProfileService
    {
        Task RegisterCustomerAsync(BaseUserCreateDTO baseUserCreateDTO);
        Task<CustomerProfileViewDTO> GetCustomerProfileAsync(string customerId);
        Task<CustomerProfileViewDTO> UpdateCustomerDetailsAsync(string customerId, CustomerProfileUpdateDTO updateDto);
        Task DeleteCustomerProfileAsync(string customerId);
        Task<List<BookingViewDTO>> ViewCustomerBookingsAsync(string customerId);
        Task<CustomerProfileViewDTO> ChangePassword(BaseUserChangePasswordDTO baseUserChangePasswordDTO);

    }
}
