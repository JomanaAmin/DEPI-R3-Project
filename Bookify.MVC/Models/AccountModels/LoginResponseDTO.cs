namespace Bookify.MVC.Models.AccountModels
{
    public class LoginResponseDTO
    {
        public string Username { get; set; }
        public bool IsSuccessful { get; set; }
        public string ValidationMessage { get; set; }
        public string AccessToken { get; set; } // The actual JWT
        public DateTime Expiration { get; set; }
    }
}
 