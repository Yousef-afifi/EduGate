using EduGate.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduGate.ViewModels.Teacher
{
    public class GenerateStudentsVM
    {
        // Navbar
        public string? TeacherName { get; set; }
        public string? Initials { get; set; }
        public int TeacherId { get; set; }

        // Form
        public int NumberOfStudents { get; set; }
        public int CourseId { get; set; }
        public NamingType NamingType { get; set; }
        public string? Prefix { get; set; }
        public bool SendWelcomeEmail { get; set; }

        // Package Info
        public int RemainingSlots { get; set; }
        public int MaxStudents{ get; set; }

        // Dropdown
        public List<SelectListItem> Courses { get; set; } = new();

        // Result
        public List<GeneratedStudentVM> GeneratedStudents { get; set; } = new();
    }
    public class GeneratedStudentVM
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
