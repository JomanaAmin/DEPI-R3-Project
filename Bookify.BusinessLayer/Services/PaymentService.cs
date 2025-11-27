using Bookify.BusinessLayer.Contracts;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;
using Bookify.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Services
{
    internal class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICartService cartService;
        private readonly IBookingService bookingService;
        private IGenericRepository<CustomerProfile> customerProfileRepository;
        public PaymentService(IUnitOfWork unitOfWork, ICartService cartService, IBookingService bookingService)
        {
            this.unitOfWork = unitOfWork;
            this.cartService = cartService;
            this.bookingService = bookingService;
            customerProfileRepository = unitOfWork.CustomerProfiles;
            // Initialize Stripe API key
            StripeConfiguration.ApiKey = "sk_test_XXXXXXXXXXXXXXXXXXXXXXXX"; // Replace with your test key
        }

        public async Task<string> CreateCheckoutSessionAsync(string customerId)
        {
            // 1️ Validate the customer exists
            var customer = await customerProfileRepository.GetAllAsQueryable().AsNoTracking().Where(c => c.CustomerId == customerId).Include(c=>c.User).FirstOrDefaultAsync();
            if (customer == null)
                throw new Exception("Customer not found.");

            // 2️ Validate cart items and calculate totals
            bool validCart = await cartService.ValidateCartItemsAsync(customerId);
            if (!validCart)
                throw new Exception("Cart contains invalid items.");

            var cart = await cartService.GetCartByUserIdAsync(customerId);
            // 3️ Create Stripe line items
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var item in cart.Items)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.PricePerNight * 100), // Stripe expects amount in cents
                        Currency = "usd", // Adjust if you want EGP / Paymob later
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.RoomId.ToString(),
                        }
                    },
                    Quantity = item.Nights
                });
            }

            // 4️ Create checkout session
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://yourfrontend.com/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://yourfrontend.com/cancel",
                CustomerEmail = customer.User.Email, // optional, prefill email
                Metadata = new Dictionary<string, string>
                {
                    { "customerId", customer.CustomerId }  // Add this line
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            // 5️ Return URL for frontend redirect
            return session.Url;
        }

        public async Task HandleWebhookAsync(string requestBody, string signatureHeader)
        {
            //Your Stripe webhook secret (from Stripe Dashboard)
            var webhookSecret = "whsec_XXXXXXXXXXXXXXXXXXXXXXXX"; // Replace with your test secret

            Event stripeEvent;

            try
            {
                // Verify the webhook signature
                stripeEvent = EventUtility.ConstructEvent(
                    requestBody,
                    signatureHeader,
                    webhookSecret
                );
            }
            catch (StripeException e)
            {
                //Invalid signature
                throw new Exception($"Stripe webhook signature verification failed: {e.Message}");
            }

            // 3️⃣ Handle the event type
            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;

                if (session == null)
                    throw new Exception("Stripe session object is null.");

                // 4️⃣ Retrieve customer ID
                // We can store customerId in metadata when creating the session
                var customerEmail = session.CustomerEmail;
                if (string.IsNullOrEmpty(customerEmail))
                    throw new Exception("Customer email not found in Stripe session.");
               // await bookingService.CreateBookingFromCartAsync(session.Metadata["customerId"]);
            }
            else
            {
                // Optional: handle other event types if needed
                Console.WriteLine($"Unhandled Stripe event type: {stripeEvent.Type}");
            }
        }

    }
}
