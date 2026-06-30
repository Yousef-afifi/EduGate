using EduGate.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ExamType Type { get; set; }
        public int? Duration { get; set; }
        public int? Total_Marks { get; set; }
        public DateTime CreatedAt {  get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey("course")]
        public int Course_Id { get; set; }
        public Course course { get; set; }
        public List<Question>? Questions { get; set; }
    }
}
