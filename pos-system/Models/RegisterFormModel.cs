using System.ComponentModel.DataAnnotations;

namespace pos_system.Models
{
    public class RegisterFormModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password minimal 6 karakter")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password dan konfirmasi harus sama")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
