using Bookify.MVC.Models;
using Bookify.MVC.Models.AccountModels;
using Bookify.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace Bookify.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService accountService;
        public AccountController(AccountService accountService)
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

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            LoginResponseDTO? login = await accountService.LoginAsync(model);

            if (login == null)
            {
                return NotFound();
            }
            return Json(login);
        }



        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(SignupRequestDTO signupRequest)
        {
            try
            {

                await accountService.RegisterCustomerAsync(signupRequest);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
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