using EduGate.ViewModels.Teacher;

namespace EduGate.Services.TeachService
{
    public interface ITeachService
    {
        Task<List<CourseVM>> GetAllCoursesAsync(int id);
        Task<CourseDetailsVM> GetCourseDetailsAsync(int courseId);
    }
}
