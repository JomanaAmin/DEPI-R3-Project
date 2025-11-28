using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.CustomExceptions;
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
                throw new UnauthorizedAccessException("User ID claim is missing from the token.");

            return userId;
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
            catch (EmailInvalidException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
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
        [Authorize (Roles ="Admin")]

        [HttpGet("admin-profile")]
        public async Task<IActionResult> GetAdminProfile()
        {
            try
            {
                var adminId = GetAuthenticatedUserId();
                var response =await adminProfileService.GetAdminProfileAsync(adminId);
                return Ok(response);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("customer-profile")]
        public async Task<IActionResult> GetCustomerProfile()
        {
            // inside GetCustomerProfile()
            Console.WriteLine("Is Authenticated = " + User.Identity?.IsAuthenticated);
            foreach (var c in User.Claims)
            {
                Console.WriteLine($"Claim: {c.Type} = {c.Value}");
            }
            try
            {
                var customerId = GetAuthenticatedUserId();
                var response =await customerProfileService.GetCustomerProfileAsync(customerId);
                return Ok(response);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("customer")]
        public async Task<IActionResult> DeleteCustomerProfile()
        {
            try
            {
                var customerId = GetAuthenticatedUserId();
                await customerProfileService.DeleteCustomerProfileAsync(customerId);
                return Ok("Customer profile deleted successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin")]
        public async Task<IActionResult> DeleteAdminProfile()
        {
            try
            {
                var adminId = GetAuthenticatedUserId();
                await adminProfileService.DeleteAdminProfileAsync(adminId);
                return Ok("Admin profile deleted successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
