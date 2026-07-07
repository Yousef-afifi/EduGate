using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public DateTime AnswerdAt { get; set; }
        public string? File_Name { get; set; }
        public string? File_Path { get; set; }
        public string? File_Type { get; set; }
        public double? File_Size { get; set; }
        [ForeignKey("student")]
        public int Student_Id { get; set; }
        public Student student { get; set; }
        [ForeignKey("question")]
        public int Question_Id { get; set; }
        public Question question { get; set; }
        [ForeignKey("choice")]
        public int? Choice_Id { get; set; }
        public Choice choice { get; set; }
    }
}
