namespace EduGate.ViewModels.Student
{
    
    
        public class TakeExamVM
        {
            public int ExamId { get; set; }
            public string ExamName { get; set; }
            public int Duration { get; set; }
            public int TotalMarks { get; set; }
            public int PassingPercentage { get; set; }
            public string StudentName { get; set; }
            public string initials { get; set; }
            public List<QuestionVM> Questions { get; set; } = new();
        }

    public class QuestionVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Mark { get; set; }

        public int? SelectedChoiceId { get; set; }

        public List<ChoiceVM> Choices { get; set; } = new();
    }

    public class ChoiceVM
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }
    
}
