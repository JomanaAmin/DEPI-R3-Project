using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICustomerProfileService customerProfileService;
        private readonly IAdminProfileService adminProfileService;
        
        public AccountController(ICustomerProfileService customerProfileService, IAdminProfileService adminProfileService)
        {
            this.customerProfileService = customerProfileService;
            this.adminProfileService = adminProfileService;
        }
        ///////////REGISTER CUSTOMER///////////
        [AllowAnonymous]
        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] BaseUserCreateDTO baseUserCreateDTO)
        {
            try
            {
                await customerProfileService.RegisterCustomerAsync(baseUserCreateDTO);
                return Ok("Customer registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            //await customerProfileService.RegisterCustomerAsync(baseUserCreateDTO);
            //return Ok("Customer registered successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] BaseUserCreateDTO baseUserCreateDTO)
        {
            try
            {
                await adminProfileService.RegisterAdminAsync(baseUserCreateDTO);
                return Ok("Admin registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/{adminId}")]
        public async Task<IActionResult> GetAdminProfile(string adminId)
        {
            try
            {
                var response=await adminProfileService.GetAdminProfileAsync(adminId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerProfile(string customerId)
        {
            try
            {
                var response=await customerProfileService.GetCustomerProfileAsync(customerId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpDelete("customer/{customerId}")]
        public async Task<IActionResult> DeleteCustomerProfile(string customerId)
        {
            try
            {
                await customerProfileService.DeleteCustomerProfileAsync(customerId);
                return Ok("Customer profile deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/{adminId}")]
        public async Task<IActionResult> DeleteAdminProfile(string adminId)
        {
            try
            {
                await adminProfileService.DeleteAdminProfileAsync(adminId);
                return Ok("Admin profile deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize (Roles = "Admin, Customer")]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] BaseUserChangePasswordDTO baseUserChangePasswordDTO)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    var response = await adminProfileService.ChangePassword(baseUserChangePasswordDTO);
                    return Ok(response);
                }
                else if (User.IsInRole("Customer"))
                {
                    var response = await customerProfileService.ChangePassword(baseUserChangePasswordDTO);
                    return Ok(response);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //[Authorize (Roles = "Admin")]
        //[HttpPost("admin/change-password")]
        //public async Task<IActionResult> ChangePassword([FromBody] BaseUserChangePasswordDTO baseUserChangePasswordDTO)
        //{
        //    try
        //    {
        //        if (User.IsInRole("Admin"))
        //        {
        //            var response = await adminProfileService.ChangePassword(baseUserChangePasswordDTO);
        //            return Ok(response);
        //        }
        //        else if (User.IsInRole("Customer"))
        //        {
        //            var response = await customerProfileService.ChangePassword(baseUserChangePasswordDTO);
        //            return Ok(response);
        //        }
        //        else
        //        {
        //            return Forbid();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
