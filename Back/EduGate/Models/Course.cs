using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey("teacher")]
        public int Teacher_Id { get; set; }
        public Teacher teacher { get; set; }
        public List<Lesson>? Lessons { get; set; }
        public List<Exam>? Exams { get; set; }
    }
}
