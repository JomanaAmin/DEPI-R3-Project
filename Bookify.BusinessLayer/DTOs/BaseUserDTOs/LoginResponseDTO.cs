using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.BaseUserDTOs
{
    public class LoginResponseDTO
    {
        public string? Username{get;set;}
        public string? AccessToken{get;set;}
        public DateTime Expiration{get;set;}
        public string Role {get;set;}
    }
}
