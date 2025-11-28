using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.CustomExceptions
{
    public class EmailInvalidException : Exception
    {

        public EmailInvalidException(string message)
            : base(message)
        {
        }
        public EmailInvalidException() : base("Email invalid.") { }
        
        
    }
}
