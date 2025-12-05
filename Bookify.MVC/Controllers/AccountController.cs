using Bookify.MVC.Models;
using Bookify.MVC.Models.AccountModels;
using Bookify.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Security.Authentication;
using System.Security.Claims;

namespace Bookify.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountMVCService accountService;
        public AccountController(AccountMVCService accountService)
        {
            this.accountService = accountService;
        }

        public IActionResult Index()
        {
            //log in or sign up page
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginRequestDTO model)
        //{
        //    LoginResponseDTO? login = await accountService.LoginAsync(model);

        //    if (login == null)
        //    {
        //        return NotFound();
        //    }
        //    return Json(login);
        //}

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            var login = await accountService.LoginAsync(model);

            if (login == null)
            {
                ModelState.AddModelError("", "Invalid login");
                return View(model);
            }

            // 1. Save JWT in secure cookie
            Response.Cookies.Append("AccessToken", login.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = login.Expiration
            });
            //HttpContext.Session.SetString("JWToken", login.AccessToken);
            //HttpContext.Session.SetString("JWTokenExpiration", login.Expiration.ToString());
            //HttpContext.Session.SetString("UserEmail", login.Username);
            //HttpContext.Session.SetString("UserRole", login.Role);

            // 2. Create MVC authentication user
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, login.Role) // this is important for [Authorize(Roles = "Admin")]

                };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(SignupRequestDTO signupRequest)
        {
            var result = await accountService.RegisterCustomerAsync(signupRequest);

            if (!result.Success)
            {
                // Add error to model state so the view can show it
                ModelState.AddModelError(string.Empty, result.ErrorMessage);

                // Return the view so the user can correct input
                return View(signupRequest);
            }

            // Optional: auto-login
            var loginRequest = new LoginRequestDTO
            {
                Username = signupRequest.Email,
                Password = signupRequest.Password
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
        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public IActionResult CheckSession()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
                return Content($"Session active! JWT token: {token.Substring(0, 20)}..."); // just first 20 chars
            else
                return Content("No active session");
        }

    }
}