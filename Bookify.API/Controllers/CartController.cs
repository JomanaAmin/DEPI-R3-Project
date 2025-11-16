using Bookify.BusinessLayer.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCart(string customerId) 
        {
            return Ok(await cartService.GetCartByUserIdAsync(customerId));
        }

    }
}
