using Bookify.BusinessLayer.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }


        [HttpPost("create-checkout-session")]
        public async Task< IActionResult> CreateCheckoutSession(string customerId)
        {
            var url = await paymentService.CreateCheckoutSessionAsync(customerId);
            return Ok(new { redirectUrl = url });
        }

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
