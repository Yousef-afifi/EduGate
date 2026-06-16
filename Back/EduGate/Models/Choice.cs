using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class Choice
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        [ForeignKey("question")]
        public int Question_Id { get; set; }
        public Question question { get; set; }
    }
}
