using EduGate.Enums;

namespace EduGate.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Account>? Accounts { get; set; }
        public List<Course>? Courses { get; set; }
    }
}