using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public ErrorType Type { get; set; }
        public Error(string code, string message, ErrorType type)
        {
            Code = code;
            Message = message;
            Type = type;
        }
        public static Error NotFound(string code, string message) => new Error(code, message, ErrorType.NotFound);
        public static Error Validation(string code, string message) => new Error(code, message, ErrorType.Validation);
        public static Error Unauthorized(string code, string message) => new Error(code, message, ErrorType.Unauthorized);
        public static Error Forbidden(string code, string message) => new Error(code, message, ErrorType.Forbidden);
        public static Error Conflict(string code, string message) => new Error(code, message, ErrorType.Conflict);
        public static Error Internal(string code, string message) => new Error(code, message, ErrorType.Internal);
    }   public enum ErrorType
    {
        NotFound,
        Validation,
        Unauthorized,
        Forbidden,
        Conflict,
        Internal
    }
}
