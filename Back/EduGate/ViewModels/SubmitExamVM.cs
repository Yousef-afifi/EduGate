namespace EduGate.ViewModels
{
    public class SubmitExamVM
    {
        public int ExamId { get; set; }

        public List<SubmitAnswerVM> Answers { get; set; } = new();
    }

    public class SubmitAnswerVM
    {
        public int QuestionId { get; set; }

        public int SelectedChoiceId { get; set; }
    }
}
