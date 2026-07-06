using EduGate.Models;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace EduGate.ViewModels.Teacher
{
    public class AddLessonVM
    {
        public string? TeacherName { get; set; }
        public string? Initials { get; set; }
        public string? CourseName { get; set; }
        public int CourseId { get; set; }
        [Required]
        public string LessonTitle { get; set; }
        [Required]
        public string VideoUrl { get; set; }
    }
    public class AddQuizVM
    {
        public string? TeacherName { get; set; }
        public string? Initials { get; set; }
        public string? CourseName { get; set; }
        public int CourseId { get; set; }
        [Required]
        public string QuizTitle { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly Time { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int TotalMark { get; set; }
        [Required]
        public int PassingScore { get; set; }
        [Required]
        public List<QuestionVM> Questions { get; set; } = new();
    }
    public class QuestionVM
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public int Mark { get; set; }
        [Required]
        public List<ChoiceVM> Choices { get; set; } = new();
    }
    public class ChoiceVM
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
    }
    public class AddAssessmentVM
    {
        public string? TeacherName { get; set; }
        public string? Initials { get; set; }
        public string? CourseName { get; set; }
        public int CourseId { get; set; }
        [Required]
        public string AssessmentTitle { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly Time { get; set; }
        [Required]
        public int TotalMark { get; set; }
        [Required]
        public int PassingScore { get; set; }
        [Required]
        public string Instruction { get; set; }
    }
    public class AddCourseVM
    {
        public string? TeacherName { get; set; }
        public string? Initials { get; set; }
        public int TeacherId { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
