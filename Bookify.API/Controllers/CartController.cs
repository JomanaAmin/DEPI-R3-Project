using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.CartDTOs;
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
        [HttpPut("{customerId}")]
        public async Task<IActionResult> GetCart(string customerId, CartItemUpdateDatesDTO cartDTO) 
        {
            return Ok(await cartService.UpdateItemDatesAsync(customerId, cartDTO));
        }
        [HttpDelete("{customerId, cartItemId}")]
        public async Task<IActionResult> GetCart(string customerId, int cartItemId) 
        {
            return Ok(await cartService.RemoveItemFromCartAsync(customerId, cartItemId));
        }

    }
}
