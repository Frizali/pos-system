using Microsoft.AspNetCore.Identity;

namespace pos_system.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
    }
}
