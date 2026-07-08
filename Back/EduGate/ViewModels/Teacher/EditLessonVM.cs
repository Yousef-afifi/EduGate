namespace EduGate.ViewModels.Teacher
{
    public class EditLessonVM
    {
        public string? TeacherName { get; set; }
        public string? Initials { get; set; }
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        public string LessonTitle { get; set; }
        public string VideoURL { get; set; }
    }
    public class EditQuizVM
    {
        public string? TeacherName { get; set; }
        public string? Initials { get; set; }
        public int QuizId { get; set; }
        public int CourseId { get; set; }
        public string QuizTitle { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int Duration { get; set; }
        public int PassingScore { get; set; }
        public int TotalMarks { get; set; }
        public List<EditQuestionVM> Questions { get; set; } = new();
    }
    public class EditQuestionVM
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int Mark { get; set; }
        public List<EditChoiceVM> Choices { get; set; } = new();
        public int CorrectChoiceId { get; set; }
    }
    public class EditChoiceVM
    {
        public int ChoiceId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
