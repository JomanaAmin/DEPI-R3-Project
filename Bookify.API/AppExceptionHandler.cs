using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookify.BusinessLayer;

namespace Bookify.API
{
    public class AppExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AppExceptionHandler> _logger;

        // Middleware must have a constructor accepting RequestDelegate
        public AppExceptionHandler(RequestDelegate next, ILogger<AppExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Middleware must expose this method
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next(httpContext);
            }
            catch (CustomException customException)
            {
                // Log the exception
                _logger.LogWarning(customException, "A custom exception occurred.");

                // Convert to ProblemDetails
                var problemDetails = ErrorToProblemDetailsMapper.ToProblemDetails(customException.error);

                // Set response
                httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/problem+json";

                // Write JSON body
                await httpContext.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Generic ProblemDetails
                var genericProblem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred."
                };

                httpContext.Response.StatusCode = genericProblem.Status ?? StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/problem+json";
                await httpContext.Response.WriteAsJsonAsync(genericProblem);
            }
        }
    }
}
