using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Video_Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey("course")]
        public int Course_Id { get; set; }
        public Course course { get; set; }
        public List<LessonMaterial>? Materials { get; set; }
    }
}
