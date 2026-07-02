using EduGate.Models;
using EduGate.ViewModels;

namespace EduGate.Services.StuServices
{
    public interface ICoursesService
    {
        Task<List<CourseVM>> GetStudentCoursesAsync(int id);
        Task<CourseDetailsVM> GetCourseDeatailsAsync(int id);
    }
}
