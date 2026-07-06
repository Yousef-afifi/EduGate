using EduGate.Models;
using EduGate.ViewModels.Student;

namespace EduGate.Services.StuServices
{
    public interface IStuService
    {
        Task<DashboardVM> GetStudentOverview(int studentId);
        Task<List<CoursesVM>> GetAllCoursesEnrolled(int studentId);
        Task<List<RecentGradesVM>> GetQuiz_AssesmentGrades(int studentId);
        Task<coursepageVM> GetStudentCoursesAsync(int id);
        Task<CourseDetailsVM> GetCourseDeatailsAsync(int studentId, int courseId);
        Task<TakeExamVM?> TakeQuiz(int studentId, int QuizId);
        Task <int> SubmitQuiz(int studentId, SubmitExamVM model);
        //------------------------------------------------------------
        Task<StudentExamsVM> GetStudentExams(int studentId);
        Task<List<UpcomingExamVM>> GetUpcomingExams(int studentId);
        Task<List<CompletedExamVM>> GetCompletedExams(int studentId);
        Task<TakeExamVM?> StartExam(int studentId, int examId);
        Task SubmitExam(int studentId, SubmitExamVM model);
        Task<List<ExamScheduleVM>> GetExamSchedule(int studentId);
    }
}
