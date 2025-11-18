using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.BaseUserDTOs.AdminProfileDTOs
{
    public class AdminProfileViewDTO
    {
        public string AdminId { get; set; } = null!;
        public string Email { get; set; } = null!; // From BaseUser
        public string FullName { get; set; } = null!; // Calculated/mapped from FirstName/LastName
        public DateTime CreatedAt { get; set; }

    }
}
