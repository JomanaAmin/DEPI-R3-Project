using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bookify.BusinessLayer.Contracts
{
    public interface IPaymentService
    {
        Task<string> CreateCheckoutSessionAsync(string customerId);
        //Responsibility:

        //Validate cart

        //Calculate total

        //Create Stripe session

        //Return redirect URL
        Task HandleWebhookAsync(string requestBody, string signatureHeader);
        //Responsibility:

        //Handle Stripe events

        //If payment successful → call BookingService

        //Used by:
        //Stripe webhook endpoint
    }
}
