using EduGate.Enums;


namespace EduGate.ViewModels.Student
{
    public class ExamScheduleVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ExamType Type { get; set; }
        public int? Duration { get; set; }
        public DateTime StartDate { get; set; }

        public string CourseName { get; set; }

    }
}