using System.ComponentModel.DataAnnotations;

namespace pos_system.Models
{
    public class LoginFormModel
    {
        [Required(ErrorMessage = "Name harus diisi")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password harus diisi")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
