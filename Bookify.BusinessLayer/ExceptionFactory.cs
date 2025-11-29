using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer
{
    public static class ExceptionFactory
    {
        public static void CreateCartNotFoundException()
        {
            Error error = new Error("Validation Error", "Cart not found", ErrorType.Validation);
            throw new CustomException(error);
        }
        public static void CreateCartItemNotFoundException()
        {
            Error error = new Error("Validation Error", "Cart item not found", ErrorType.Validation);
            throw new CustomException(error);
        }
        public static void CreateRoomNotFoundException()
        {
            Error error = new Error("Validation Error", "Room not found", ErrorType.Validation);
            throw new CustomException(error);
        }
        public static void CreateRoomTypeNotFoundException()
        {
            Error error = new Error("Validation Error", "Room type not found", ErrorType.Validation);
            throw new CustomException(error);
        }
        public static void ProfileNotFoundException(string role)
        {
            Error error = new Error("Validation Error", $"{role} profile not found", ErrorType.Validation);
            throw new CustomException(error);
        }
        public static void EmailAlreadyInUseException()
        {
            Error error = new Error("Validation Error", "Email is already in use.", ErrorType.Validation);
            throw new CustomException(error);
        }
        public static void IncorrectEmailException()
        {
            Error error = new Error("Validation Error", "Email used is incorrect.", ErrorType.Validation);
            throw new CustomException(error);
        }
        public static void UserCreationException()
        {
            Error error = new Error("Internal Error", "Could not create user succeffully.", ErrorType.Internal);
            throw new CustomException(error);
        }
        public static void UnauthorizedException(string msg)
        {
            Error error = new Error("Unauthorized Error", $"{msg}", ErrorType.Unauthorized);
            throw new CustomException(error);
        }
        public static void Exception(string msg)
        {
            Error error = new Error("Unauthorized Error", $"{msg}", ErrorType.Unauthorized);
            throw new CustomException(error);
        }
        public static void CartInvalidItemsException()
        {
            Error error = new Error("Validation Error", "Cart contains invalid items.", ErrorType.Validation);
            throw new CustomException(error);
        }
        public static void LoginFailedException()
        {
            Error error = new Error("Validation Error", "Invalid login. Try again.", ErrorType.Validation);
            throw new CustomException(error);
        }
    }
}
