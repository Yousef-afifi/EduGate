using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        [ForeignKey("student")]
        public int Student_Id { get; set; }
        public Student student { get; set; }
        [ForeignKey("course")]
        public int Course_Id { get; set; }
        public Course course { get; set; }
    }
}
