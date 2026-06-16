using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public DateTime AnswerdAt { get; set; }
        [ForeignKey("student")]
        public int Student_Id { get; set; }
        public Student student { get; set; }
        [ForeignKey("question")]
        public int Question_Id { get; set; }
        public Question question { get; set; }
        [ForeignKey("choice")]
        public int Choice_Id { get; set; }
        public Choice choice { get; set; }
    }
}
