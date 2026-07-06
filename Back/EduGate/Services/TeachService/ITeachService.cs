using EduGate.ViewModels.Teacher;

namespace EduGate.Services.TeachService
{
    public interface ITeachService
    {
        Task<CoursePageVM> GetAllCoursesAsync(int id);
        Task<CourseDetailsVM> GetCourseDetailsAsync(int courseId);
        Task<StudentPageVM> GetAllStudentsAsync(int id);
        Task<AddLessonVM> GetAddLesson(int Id);
        Task AddLesson(AddLessonVM model);
        Task<AddQuizVM> GetAddQuiz(int courseId);
        Task AddQuiz(AddQuizVM model);
    }
}
