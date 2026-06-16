using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class ExamAttempt
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public int Score { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        [ForeignKey("student")]
        public int Student_Id { get; set; }
        public Student student { get; set; }
        [ForeignKey("exam")]
        public int Exam_Id { get; set; }
        public Exam exam { get; set; }
    }
}
