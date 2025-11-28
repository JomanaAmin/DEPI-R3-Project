using System.ComponentModel.DataAnnotations;

namespace Bookify.MVC.Models.AccountModels
{
    public class SignupRequestDTO
    {

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
    
    }
}
