using EduGate.Models;

namespace EduGate.ViewModels.Teacher
{
    public class CoursePageVM
    {
        public string TeacherName { get; set; }
        public string Initials { get; set; }
        public List<CourseVM> Courses { get; set; }
    }
    public class CourseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public int LessonCount { get; set; }
        public int StudentConut { get; set; }
    }
    public class CourseDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeacherName { get; set; }
        public string Initials { get; set; }
        public int LessonCount { get; set; }
        public int StudentConut { get; set; }
        public List<LessonVM> Lessons { get; set; }
        public List<QuizVM> Quizzes { get; set; }
        public List <AssessmentVM> Assessments { get; set; }
    }
    public class LessonVM
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public List<LessonMaterialVM> Materials { get; set; }
    }
    public class QuizVM
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public int QusetionConut { get; set; }
    }
    public class AssessmentVM
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public DateTime DueDate { get; set; }
    }
    public class LessonMaterialVM
    {
        public string Name { get; set;}
        public string Url { get; set;}
        public double File_Size { get; set;}
    }
}
