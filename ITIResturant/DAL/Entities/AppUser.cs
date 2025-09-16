
using Microsoft.AspNetCore.Identity;
using Restaurant.DAL.Enum;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.DAL.Entities
{
    public abstract class AppUser : IdentityUser<int>
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Required]
        [Phone]
        public override string PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public UserTypeEnum UserType { get; set; }

        public string? Address { get; set; }
    }
}
