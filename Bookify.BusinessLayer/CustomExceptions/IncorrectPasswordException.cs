using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.CustomExceptions
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException(string message)
            : base(message)
        {
        }
        public IncorrectPasswordException() : base("Password is incorrect. Please enter another password.") { }


    }
}
