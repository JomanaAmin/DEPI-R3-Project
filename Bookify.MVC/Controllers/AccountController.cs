using Bookify.MVC.Contracts;
using Bookify.MVC.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace Bookify.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public IActionResult Index()
        {
            //log in or sign up page
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            var result = await accountService.LoginAsync(model);
            if (result.IsSuccessful)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, result.ValidationMessage ?? "Login failed");
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCustomer(SignupRequestDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await accountService.CreateAccountCustomerAsync(model);
            if (result.IsSuccessful)
            {
                // AccountService attempts automatic login and sets cookie.
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, result.ValidationMessage ?? "Registration failed");
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(SignupRequestDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await accountService.CreateAccountAdminAsync(model);
            if (result.IsSuccessful)
            {
                // AccountService attempts automatic login and sets cookie.
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, result.ValidationMessage ?? "Registration failed");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Remove the stored JWT cookie used by the AuthTokenHandler
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> CustomerProfile()
        {
            try
            {
                var profile = await accountService.ViewCustomerProfile();
                return View(profile);
            }
            catch (AuthenticationException)
            {
                // Session expired / unauthorized -> redirect to login
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AdminProfile()
        {
            try
            {
                var profile = await accountService.ViewAdminProfile();
                return View(profile);
            }
            catch (AuthenticationException)
            {
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}