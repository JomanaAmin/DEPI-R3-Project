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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RegisterCustomer(SignupRequestDTO model)
        //{
        //    ApiResponse<SignupResponseDTO> result = await accountService.CreateAccountCustomerAsync(model);

        //    if (result.IsSuccess)
        //    {
        //        // Success: result.Data contains LoginResponseDTO
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // Error: result.Error contains ErrorDTO
        //    ModelState.AddModelError("", result.Error.Message);

        //    // Return the same form so user can fix input
        //    return View(model);
        //}

        //[HttpGet]
        //public IActionResult RegisterAdmin()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> RegisterAdmin(SignupRequestDTO model)
        //{
        //    ApiResponse<SignupResponseDTO> result = await accountService.CreateAccountAdminAsync(model);

        //    if (result.IsSuccess)
        //    {
        //        // Success: result.Data contains LoginResponseDTO
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // Error: result.Error contains ErrorDTO
        //    ModelState.AddModelError("", result.Error.Message);

        //    // Return the same form so user can fix input
        //    return View(model);

        //}

        //[HttpPost]
        //public IActionResult Logout()
        //{
        //    // Remove the stored JWT cookie used by the AuthTokenHandler
        //    Response.Cookies.Delete("AuthToken");
        //    return RedirectToAction("Index", "Home");
        //}

        //[HttpGet]
        //public async Task<IActionResult> CustomerProfile()
        //{

        //    var response = await accountService.ViewCustomerProfile();
        //    if (response.IsSuccess)
        //        return View(response);
        //    ModelState.AddModelError("",response.Error.Message);
        //    return View(response);


        //}

        //[HttpGet]
        //public async Task<IActionResult> AdminProfile()
        //{
        //    try
        //    {
        //        var profile = await accountService.ViewAdminProfile();
        //        return View(profile);
        //    }
        //    catch (AuthenticationException)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = ex.Message;
        //        return RedirectToAction("Index", "Home");
        //    }
        //}
    }
}