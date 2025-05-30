using System.ComponentModel.DataAnnotations;

namespace pos_system.Models
{
    public class RegisterFormModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password minimal 6 karakter")]
        public string Password { get; set; } = String.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password dan konfirmasi harus sama")]
        public string ConfirmPassword { get; set; } = String.Empty;

        public string Role { get; set; } = "User";
    }
}
