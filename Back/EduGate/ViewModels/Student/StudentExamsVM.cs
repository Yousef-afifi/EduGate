namespace EduGate.ViewModels.Student
{
    public class StudentExamsVM
    {
        public string StudentName { get; set; }
        public string initials { get; set; }
        public List<UpcomingExamVM> UpcomingExams { get; set; }
        public List<CompletedExamVM> CompletedExams { get; set; }
    }

   
    public class UpcomingExamVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public int Duration { get; set; }
        public int TotalMarks { get; set; }
        public int QuestionCount { get; set; }
        public int PassingPercentage { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class CompletedExamVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public int Duration { get; set; }
        public int TotalMarks { get; set; }
        public int QuestionCount { get; set; }
        public double Score { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
