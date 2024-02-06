using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public record LoginDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
