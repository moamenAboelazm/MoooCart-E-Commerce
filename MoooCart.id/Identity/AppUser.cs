using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace MoooCart.id.Identity
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        public string FullName { get; set; }
        public string? ProfileImgUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
