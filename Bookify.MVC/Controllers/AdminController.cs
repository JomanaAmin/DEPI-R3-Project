using Bookify.MVC.Models.AccountModels;
using Bookify.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.MVC.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        private readonly AdminService adminService;
        private readonly AccountMVCService accountService;
        public AdminController(AdminService adminService, AccountMVCService accountService)
        {
            this.adminService = adminService;
            this.accountService = accountService;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(SignupRequestDTO adminRequest)
        {
            var result = await adminService.RegisterAdminAsync(adminRequest);

            if (!result.Success)
            {
                // Add error to model state so the view can show it
                ModelState.AddModelError(string.Empty, result.ErrorMessage);

                // Return the view so the user can correct input
                return View(adminRequest);
            }

            // Optional: auto-login
            var loginRequest = new LoginRequestDTO
            {
                Username = adminRequest.Email,
                Password = adminRequest.Password
            };
            var loginResponse = await accountService.LoginAsync(loginRequest);

            if (loginResponse != null)
            {
                HttpContext.Session.SetString("JWToken", loginResponse.AccessToken);
                HttpContext.Session.SetString("JWTokenExpiration", loginResponse.Expiration.ToString());
                HttpContext.Session.SetString("UserEmail", loginResponse.Username);
                HttpContext.Session.SetString("UserRole", loginResponse.Role);

            }

            return RedirectToAction("Index", "Home");
        }
    }
}
