using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public record RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one NonAlphanumeric .")]
        public string Password { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public AddressDto Address { get; set; }

    }
}
