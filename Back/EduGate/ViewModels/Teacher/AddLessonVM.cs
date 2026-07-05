using System.ComponentModel.DataAnnotations;

namespace EduGate.ViewModels.Teacher
{
    public class AddLessonVM
    {
        public string? TeacherName { get; set; }
        public string? Initials { get; set; }
        public string? CourseName { get; set; }
        public int CourseId { get; set; }
        [Required]
        public string LessonTitle { get; set; }
        [Required]
        public string VideoUrl { get; set; }
    }
}
