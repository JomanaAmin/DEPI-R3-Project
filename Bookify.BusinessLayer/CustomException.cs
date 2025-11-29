using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer
{
    public class CustomException : Exception
    {
        public Error error { get; set; }
        public CustomException(Error error) : base(error.Message)
        {
            this.error = error;
        }
    }
}
