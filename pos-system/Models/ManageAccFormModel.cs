namespace pos_system.Models
{
    public class ManageAccFormModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
