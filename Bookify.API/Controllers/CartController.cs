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
                throw new UnauthorizedAccessException("User ID claim is missing from the token.");

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
            try
            {
                var customerId = GetAuthenticatedUserId();
                return Ok(await cartService.GetCartByUserIdAsync(customerId));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPut("edit-cart")]
        public async Task<IActionResult> UpdateCartItem( CartItemUpdateDatesDTO cartDTO) //string customerId,
        {
            try
            {
                var customerId = GetAuthenticatedUserId();
                return Ok(await cartService.UpdateItemDatesAsync(customerId, cartDTO));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        //[Authorize(Roles = "Customer")]
        [HttpDelete("cart-items/{cartItemId}")]
        public async Task<IActionResult> DeleteItemFromCart( int cartItemId)// 
        {
            try
            {
                var customerId = GetAuthenticatedUserId();
                return Ok(await cartService.RemoveItemFromCartAsync(customerId, cartItemId));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
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
            try
            {
                var customerId = GetAuthenticatedUserId();
                await cartService.AddItemToCartAsync(customerId, cartItemDTO);
                return Ok($"Added room {cartItemDTO.RoomId} to cart, from {cartItemDTO.CheckInDate} to {cartItemDTO.CheckOutDate}");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        //[Authorize(Roles = "Customer")]
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()//
        {
            try
            {
                var customerId = GetAuthenticatedUserId();
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
