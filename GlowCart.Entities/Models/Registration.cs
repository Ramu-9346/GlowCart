using System.ComponentModel.DataAnnotations;

namespace GlowCart.Entities.Models
{
    public class Registration
    {
        [Required]
        public string? FullName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required, Phone]
        public string? Phone { get; set; }

        [Required]
        public string? Address { get; set; }
    }
}
