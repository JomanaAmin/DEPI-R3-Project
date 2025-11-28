namespace Bookify.MVC.Models.AccountModels
{
    public class AdminAccountViewDTO
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool IsSuccessful { get; set; }
        public string? ValidationMessage { get; set; }
    }
}
