using EduGate.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EduGate.ViewModels.Teacher
{
    public class ExamPageVM
    {
        public string TeacherName { get; set; }
        public string Initials { get; set; }
        public List<ExamListItemVM> Exams { get; set; } = new();
    }

    public class ExamListItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ExamType Type { get; set; }
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public int? Duration { get; set; }
        public int QuestionCount { get; set; }
        public int EnrolledCount { get; set; }
        public bool IsCompleted => StartDate < DateTime.Now;
    }

    public class AddExamVM
    {
        public string? TeacherName { get; set; }
        public string? Initials { get; set; }
        public List<SelectListItem> Courses { get; set; } = new();

        [Required]
        public int CourseId { get; set; }
        [Required]
        public string ExamTitle { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly Time { get; set; }
        [Required]
        public int Duration { get; set; }
       
        [Required]
        public int PassingScore { get; set; }
        public List<QuestionVM> Questions { get; set; } = new();
    }

    public class EditExamVM
    {
        public int ExamId { get; set; }
        public string? TeacherName { get; set; }
        public string? Initials { get; set; }
        public string? CourseName { get; set; }
        public int CourseId { get; set; }

        [Required]
        public string ExamTitle { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly Time { get; set; }
        [Required]
        public int Duration { get; set; }
        
        [Required]
        public int PassingScore { get; set; }
        public List<QuestionVM> Questions { get; set; } = new();
    }
}