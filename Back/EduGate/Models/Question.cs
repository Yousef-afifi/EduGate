using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Mark { get; set; }
        [ForeignKey("exam")]
        public int Exam_Id { get; set; }
        public Exam exam { get; set; }
        public List<Choice>? Choices { get; set; }
    }
}
