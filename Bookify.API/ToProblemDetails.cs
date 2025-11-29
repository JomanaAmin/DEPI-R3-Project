using Bookify.BusinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API
{
    public static class ErrorToProblemDetailsMapper
    {
        public static ProblemDetails ToProblemDetails(Error error) 
        {
            var problemDetails = new ProblemDetails
            {
                Title = error.Code,
                Detail = error.Message,
                Status = GetStatusCode(error.Type)
            };
            return problemDetails;
        }
        public static int GetStatusCode(ErrorType errorType) 
        {
            switch (errorType) 
            {
                case ErrorType.NotFound:
                    return StatusCodes.Status404NotFound;
                case ErrorType.Validation:
                    return StatusCodes.Status400BadRequest;
                case ErrorType.Unauthorized:
                    return StatusCodes.Status401Unauthorized;
                case ErrorType.Forbidden:
                    return StatusCodes.Status403Forbidden;
                case ErrorType.Conflict:
                    return StatusCodes.Status409Conflict;
                case ErrorType.Internal:
                    return StatusCodes.Status500InternalServerError;
                default:
                    return StatusCodes.Status500InternalServerError ;
            }
        }
    }
}
