using Bookify.BusinessLayer;
using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.CartDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bookify.API.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
      
        private string GetAuthenticatedUserId()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                Error error = new Error("Unauthorized Error", "User ID claim is missing from the token.", ErrorType.Unauthorized);
                throw new CustomException(error);
            }
            return userId;
        }
        [Authorize(Roles = "Customer,Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("test")]
        public IActionResult Test() => Ok("You reached the endpoint");

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {  
            var customerId = GetAuthenticatedUserId();
            return Ok(await cartService.GetCartByUserIdAsync(customerId));
        }
        [HttpPut("edit-cart")]
        public async Task<IActionResult> UpdateCartItem( CartItemUpdateDatesDTO cartDTO) //string customerId,
        {
            var customerId = GetAuthenticatedUserId();
            return Ok(await cartService.UpdateItemDatesAsync(customerId, cartDTO));
        }
        [HttpDelete("cart-items/{cartItemId}")]
        public async Task<IActionResult> DeleteItemFromCart( int cartItemId)// 
        { 
            var customerId = GetAuthenticatedUserId();
            return Ok(await cartService.RemoveItemFromCartAsync(customerId, cartItemId));
        }
        [HttpDelete("clear-cart")]
        public async Task<IActionResult> ClearFromCart()// string customerId
        {
            var customerId = GetAuthenticatedUserId();
            await cartService.ClearCartAsync(customerId);
            return Ok();
        }
        //[Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> AddItemToCart( CartAddItemDTO cartItemDTO) //
        {
            var customerId = GetAuthenticatedUserId();
            await cartService.AddItemToCartAsync(customerId, cartItemDTO);
            return Ok($"Added room {cartItemDTO.RoomId} to cart, from {cartItemDTO.CheckInDate} to {cartItemDTO.CheckOutDate}"); 
        }
        //[Authorize(Roles = "Customer")]
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()//
        {
            var customerId = GetAuthenticatedUserId();
            var response = await cartService.CalculateCheckoutSummaryAsync(customerId);
            return Ok(response);
            
        }
    }
}
