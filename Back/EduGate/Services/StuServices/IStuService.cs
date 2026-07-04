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
        Task<TakeExamVM?> StartExam(int studentId, int examId);
        Task SubmitExam(int studentId, SubmitExamVM model);
    }
}
