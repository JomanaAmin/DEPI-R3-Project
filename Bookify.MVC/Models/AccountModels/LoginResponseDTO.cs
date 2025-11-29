namespace Bookify.MVC.Models.AccountModels
{
    public class LoginResponseDTO
    {
        public string? Username { get; set; }
        public string? AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
 