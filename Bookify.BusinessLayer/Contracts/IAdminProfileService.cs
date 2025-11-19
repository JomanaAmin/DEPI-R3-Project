using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs.AdminProfileDTOs;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs.CustomerProfileDTOs;
using Bookify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Contracts
{
    public interface IAdminProfileService
    {
        Task RegisterAdminAsync(BaseUserCreateDTO baseUserCreateDTO);
        Task<AdminProfileViewDTO> GetAdminProfileAsync(string adminId);
        Task<AdminProfileViewDTO> UpdateAdminDetailsAsync(string adminId, AdminProfileUpdateDTO updateDto);
        Task DeleteAdminProfileAsync(string adminId);
        Task<AdminProfileViewDTO> ChangePassword(BaseUserChangePasswordDTO baseUserChangePasswordDTO);

    }
}
