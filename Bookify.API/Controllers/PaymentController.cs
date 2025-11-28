using Bookify.BusinessLayer.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("create-checkout-session")]
        public async Task< IActionResult> CreateCheckoutSession()
        {

            var customerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(customerId))
                throw new UnauthorizedAccessException("User ID claim is missing from the token.");
            var url = await paymentService.CreateCheckoutSessionAsync(customerId);
            return Ok(new { redirectUrl = url });
        }
        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            // Stripe requires the raw JSON payload & signature header
            using var reader = new StreamReader(HttpContext.Request.Body);
            var jsonBody = await reader.ReadToEndAsync();

            var signatureHeader = Request.Headers["Stripe-Signature"];

            await paymentService.HandleWebhookAsync(jsonBody, signatureHeader);

            // Stripe expects a 200 OK always
            return Ok();
        }
    }
}
