using Bookify.MVC.Models.CartDTO;
using Bookify.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.MVC.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private readonly CartMVCService cartService;
        public CartController(CartMVCService cartService)
        {
            this.cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await cartService.ViewCartAsync();

            if (!result.Success)
            {
                // Add the error message to the ModelState so the view can display it
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                // You can also return an empty view or a view model with no items
                return View(new CartViewDTO());
            }

            // Pass the retrieved cart data to the view
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(CartAddItemDTO cartAddItem) 
        {
            var result = await cartService.AddToCartAsync(cartAddItem);

            if (!result.Success) 
            {            
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Index", "RoomDetails", new { id = cartAddItem.RoomId });
            }
            TempData["Success"] = result.ErrorMessage;
            return RedirectToAction("Index","Cart");

        }
    }
}
