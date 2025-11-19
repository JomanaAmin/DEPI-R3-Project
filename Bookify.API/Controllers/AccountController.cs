using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers
{
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
                return BadRequest(ex.Message);
            }
            //await customerProfileService.RegisterCustomerAsync(baseUserCreateDTO);
            //return Ok("Customer registered successfully.");
        }
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
                return BadRequest(ex.Message);
            }
        }
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
                return BadRequest(ex.Message);
            }
        }
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
                return BadRequest(ex.Message);
            }
        }
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
                return BadRequest(ex.Message);
            }
        }
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
                return BadRequest(ex.Message);
            }
        }
    }
}
