using EduGate.Enums;

namespace EduGate.ViewModels.Student
{
    public class CourseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeacherName { get; set; }
        public int LessonCount { get; set; }
        public int ExamCount { get; set; }

        public int ProgressPercentage { get; set; }
    }
    public class coursepageVM
    {
        public string initials { get; set; }
        public string StudentName { get; set; }
        public List<CourseVM> courses { get; set; } = new List<CourseVM>();
    }
    public class CourseDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeacherName { get; set; }
        public string StudentName { get; set; }
        public string initials { get; set; }
        public int ProgressPrecentage { get; set; }

        public List<LessonVM> Lessons { get; set; } = new List<LessonVM>();
        public List<QuizVM> Quizzes { get; set; } = new List<QuizVM>();
        public List<AssessmentVM> Assessments { get; set; } = new List<AssessmentVM>();
        public List<LessonMaterialVM> Materials { get; set; } = new List<LessonMaterialVM>();
    }
    public class LessonVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Video_Url { get; set; }
        public List<LessonMaterialVM> Materials { get; set; }
    }
    public class QuizVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuestionCount { get; set; }
        public string score { get; set; }
        public string Status { get; set; }
       
    }
    public class AssessmentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string score { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }
        
    }
    public class LessonMaterialVM
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public double FileSize { get; set; }
        public string DownloadUrl { get; set; }
    }

}
