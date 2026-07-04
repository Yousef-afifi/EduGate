using EduGate.Models;

namespace EduGate.ViewModels.Teacher
{
    public class CourseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public int LessonCount { get; set; }
    }
    public class CourseDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LessonCount { get; set; }
        public List<LessonVM> Lessons { get; set; }
        public List<QuizVM> Quizzes { get; set; }
        public List <AssessmentVM> Assessments { get; set; }
    }
    public class LessonVM
    {
        public string Name { get; set;}
        public List<LessonMaterialVM> Materials { get; set; }
    }
    public class QuizVM
    {
        public string Name { get; set;}
        public int QustionConut { get; set; }
    }
    public class AssessmentVM
    {
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
