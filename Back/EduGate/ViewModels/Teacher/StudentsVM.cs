using EduGate.Enums;
using EduGate.Models;

namespace EduGate.ViewModels.Teacher
{
    public class StudentPageVM
    {
        public string TeacherName { get; set; }
        public string Initials { get; set; }
        public List<StudentVM> Students { get; set; }
    }
    public class StudentVM
    {
        public string StudentName { get; set; }
        public string Initials { get; set; }
        public string StudentEmail { get; set; }
        public int ECourseCount { get; set; }
        public AccountStatus Status { get; set; }
        public DateOnly Joined { get; set; }
    }
}
