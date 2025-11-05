using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bookify.DAL.Entities;

namespace Bookify.DAL.Entities
{
    public class AdminProfile
    {
        [Key]
        [ForeignKey("User")]
        public string AdminId { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Navigation Properties
        public BaseUser User { get; set; } = null!;
    }
}