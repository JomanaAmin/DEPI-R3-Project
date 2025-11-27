using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJwtService jwtService;
        public LoginController(IJwtService jwtService)
        {
            this.jwtService = jwtService;
        }

        [HttpPost("admin")]
        public async Task<IActionResult> AdminLogin(LoginRequestDTO loginRequest)
        {
            try
            {
                return Ok(await jwtService.Authenticate(loginRequest));
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }

        }
    }
}
