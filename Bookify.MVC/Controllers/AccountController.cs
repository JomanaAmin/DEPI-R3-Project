using Bookify.MVC.Contracts;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
    }
}
