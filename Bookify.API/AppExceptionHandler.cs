using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Bookify.BusinessLayer;
namespace Bookify.API
{
    public class AppExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is CustomException customException) 
            {
                var problemDetails = ErrorToProblemDetailsMapper.ToProblemDetails(customException.error);
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                return true;
            }
            // For security, always return a generic 500 without internal details
            var genericProblem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An internal server error occurred",
                Detail = "A critical, unexpected issue prevented the request from being processed. Please try again later."
            };

            // The Status property on ProblemDetails is nullable (int?), so we need a null check/coalesce before accessing Value
            // In this case, we just assigned 500, so we can use .Value or the Status code directly.
            httpContext.Response.StatusCode = genericProblem.Status ?? StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(genericProblem, cancellationToken);

            return true; // Indicate that we have handled the exception
        }
    }
}
