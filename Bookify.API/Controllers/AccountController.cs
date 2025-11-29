using Bookify.BusinessLayer;
using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Bookify.API.Controllers
{
    [Authorize]
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
        private string GetAuthenticatedUserId()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                Error error = new Error("Unauthorized Error", "User ID claim is missing from the token.", ErrorType.Unauthorized);
                throw new CustomException(error);
            }

            return userId;
        }
        ///////////REGISTER CUSTOMER///////////
        [AllowAnonymous]
        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] BaseUserCreateDTO baseUserCreateDTO)
        {
            await customerProfileService.RegisterCustomerAsync(baseUserCreateDTO);
            return Ok("Customer registered successfully.");
        }
        [AllowAnonymous]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] BaseUserCreateDTO baseUserCreateDTO)
        {
            await adminProfileService.RegisterAdminAsync(baseUserCreateDTO);
            return Ok("Admin registered successfully.");
        }
        [Authorize (Roles ="Admin")]

        [HttpGet("admin-profile")]
        public async Task<IActionResult> GetAdminProfile()
        {
            var adminId = GetAuthenticatedUserId();
            var response =await adminProfileService.GetAdminProfileAsync(adminId);
            return Ok(response);
        }
        [HttpGet("customer-profile")]
        public async Task<IActionResult> GetCustomerProfile()
        {
            var customerId = GetAuthenticatedUserId();
            var response =await customerProfileService.GetCustomerProfileAsync(customerId);
            return Ok(response);
        }
        [HttpDelete("customer")]
        public async Task<IActionResult> DeleteCustomerProfile()
        {
            var customerId = GetAuthenticatedUserId();
            await customerProfileService.DeleteCustomerProfileAsync(customerId);
            return Ok("Customer profile deleted successfully.");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin")]
        public async Task<IActionResult> DeleteAdminProfile()
        {
            var adminId = GetAuthenticatedUserId();
            await adminProfileService.DeleteAdminProfileAsync(adminId);
            return Ok("Admin profile deleted successfully.");
        }
    }
}

