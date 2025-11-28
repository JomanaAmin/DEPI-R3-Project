
using Bookify.MVC.Models.BookingModels;

namespace Bookify.MVC.Models.AccountModels
{
    public class CustomerAccountViewDTO
    {
        //public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool IsSuccessful { get; set; }
        public string? ValidationMessage { get; set; }
        //public List<BookingViewDTO>? Bookings { get; set; }
    }
}
