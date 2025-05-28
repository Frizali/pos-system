namespace pos_system.Models
{
    public class LoginFormModel
    {
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public bool RememberMe { get; set; } = false;
    }
}
