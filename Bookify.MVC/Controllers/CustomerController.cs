using Microsoft.AspNetCore.Mvc;

namespace Bookify.MVC.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
