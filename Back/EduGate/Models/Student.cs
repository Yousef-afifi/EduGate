using EduGate.Enums;

namespace EduGate.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
    }
}
