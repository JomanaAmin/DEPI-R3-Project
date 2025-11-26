using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.CartDTOs;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> UpdateCartItem(string customerId, CartItemUpdateDatesDTO cartDTO) 
        {
            return Ok(await cartService.UpdateItemDatesAsync(customerId, cartDTO));
        }
        [HttpDelete("/{customerId}/cart-items/{cartItemId}")]
        public async Task<IActionResult> DeleteItemFromCart(string customerId, int cartItemId) 
        {
            return Ok(await cartService.RemoveItemFromCartAsync(customerId, cartItemId));
        }
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> ClearFromCart(string customerId) 
        {
            await cartService.ClearCartAsync(customerId);
            return Ok();
        }
        [HttpPost("{customerId}")]
        public async Task<IActionResult> AddItemToCart(string customerId, CartAddItemDTO cartItemDTO) 
        {
            try
            {
                await cartService.AddItemToCartAsync(customerId, cartItemDTO);
                return Ok($"Added room {cartItemDTO.RoomId} to cart, from {cartItemDTO.CheckInDate} to {cartItemDTO.CheckOutDate}");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("checkout")]
        //[Authorize(Roles = "Customer")] // Only Customers can check out
        public async Task<IActionResult> Checkout(string customerId)
        {
            try
            {

                var response = await cartService.CalculateCheckoutSummaryAsync(customerId);

                return Ok(response);
            }
            catch (Exception ex)
            { 
     
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
