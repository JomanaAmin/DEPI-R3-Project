using Microsoft.AspNetCore.Identity;

namespace Bookify.DAL.Entities
{
    public class BaseUser : IdentityUser
    {
        public CustomerProfile? CustomerProfile { get; set; }
        public AdminProfile? AdminProfile { get; set; }
    }
}