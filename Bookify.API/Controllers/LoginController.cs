using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginRequestDTO loginRequest)
        {
            string defaultMessage = "Invalid credentials, please try again.";
            return Ok(await jwtService.Authenticate(loginRequest));
        }
    }
}
