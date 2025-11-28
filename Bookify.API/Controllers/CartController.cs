using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.CartDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bookify.API.Controllers
{
    //[Authorize]
    //[Authorize(Roles = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        //private string GetAuthenticatedUserId()
        //{
        //    Console.WriteLine("Is Authenticated = " + User.Identity.IsAuthenticated);

        //    // The ClaimTypes.NameIdentifier (or sometimes ClaimTypes.Name) holds the User ID 
        //    // that was put into the JWT payload when the token was created.
        //    var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        //    // This check should technically be redundant if [Authorize] is used, 
        //    // but it's good practice.
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        // This indicates a severe authentication failure or misuse of the method.
        //        throw new UnauthorizedAccessException("User ID claim is missing from the token.");
        //    }

        //    return userId;
        //}
        // [Authorize(Roles = "Customer,Admin")]
        [Authorize]
        [HttpGet("test")]
        public IActionResult Test() => Ok("You reached the endpoint");

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCart(string customerId)
        {
            try
            {
                //var customerId = GetAuthenticatedUserId();
                return Ok(await cartService.GetCartByUserIdAsync(customerId));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPut("edit-cart/{customerId}")]
        public async Task<IActionResult> UpdateCartItem(string customerId, CartItemUpdateDatesDTO cartDTO) //string customerId,
        {
            try
            {
                //var customerId = GetAuthenticatedUserId();
                return Ok(await cartService.UpdateItemDatesAsync(customerId, cartDTO));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        //[Authorize(Roles = "Customer")]
        [HttpDelete("cart-items/{cartItemId}/{customerId}")]
        public async Task<IActionResult> DeleteItemFromCart(string customerId, int cartItemId)// 
        {
            try
            {
                //var customerId = GetAuthenticatedUserId();
                return Ok(await cartService.RemoveItemFromCartAsync(customerId, cartItemId));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("clear-cart/{customerId}")]
        public async Task<IActionResult> ClearFromCart(string customerId)// string customerId
        {

            //var customerId = GetAuthenticatedUserId();
            await cartService.ClearCartAsync(customerId);
            return Ok();
        }
        //[Authorize(Roles = "Customer")]
        [HttpPost("{customerId}")]
        public async Task<IActionResult> AddItemToCart(string customerId, CartAddItemDTO cartItemDTO) //
        {
            try
            {
                //var customerId = GetAuthenticatedUserId();
                await cartService.AddItemToCartAsync(customerId, cartItemDTO);
                return Ok($"Added room {cartItemDTO.RoomId} to cart, from {cartItemDTO.CheckInDate} to {cartItemDTO.CheckOutDate}");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        //[Authorize(Roles = "Customer")]
        [HttpPost("checkout/{customerId}")]
        public async Task<IActionResult> Checkout(string customerId)//
        {
            try
            {
                //var customerId = GetAuthenticatedUserId();
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
