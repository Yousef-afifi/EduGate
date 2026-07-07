using System.ComponentModel.DataAnnotations;

namespace EduGate.ViewModels.Student
{
    public class SubmitAssessmentVM
    {
        public string? StudentName { get; set; }
        public string? Initials { get; set; }
        public string? AssessmentTitle { get; set; }
        public string? Instructions { get; set; }
        public int? TotalMark { get; set; }
        public DateTime? DueDate { get; set; }
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        public int AssessmentId { get; set; }
        public int CourseId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }    
}
