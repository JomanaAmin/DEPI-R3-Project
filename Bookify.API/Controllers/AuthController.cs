using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService jwtService;
        public AuthController(IJwtService jwtService)
        {
            this.jwtService = jwtService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO) 
        {
            try
            {
                var response = await jwtService.Authenticate(loginRequestDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
