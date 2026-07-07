using System.ComponentModel.DataAnnotations;

namespace EduGate.ViewModels.Shared
{
    public class SettingsVM
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        public string? TeacherName { get; set; }
        public string? Initials { get; set; }

        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmNewPassword { get; set; }
    }
}
