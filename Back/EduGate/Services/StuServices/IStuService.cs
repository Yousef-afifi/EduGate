using EduGate.Models;
using EduGate.ViewModels;

namespace EduGate.Services.StuServices
{
    public interface IStuService
    {
        Task<List<CourseVM>> GetStudentCoursesAsync(int id);
        Task<CourseDetailsVM> GetCourseDeatailsAsync(int id);
     
        //------------------------------------------------------------
        Task<StudentExamsVM> GetStudentExams(int studentId);
        Task<List<UpcomingExamVM>> GetUpcomingExams(int studentId);
        Task<List<CompletedExamVM>> GetCompletedExams(int studentId);
    }
}
