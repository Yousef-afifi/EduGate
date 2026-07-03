using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class LessonMaterial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string File_Path { get; set; }
        public string File_Type { get; set; }
        public double File_Size { get; set; }
        [ForeignKey("lesson")]
        public int Lesson_Id { get; set; }
        public Lesson lesson { get; set; }
    }
}
