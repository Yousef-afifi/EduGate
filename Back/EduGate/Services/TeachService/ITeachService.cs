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
        Task<AddAssessmentVM> GetAddAssessment(int courseId);
        Task AddAssessment(AddAssessmentVM model);
        Task<AddCourseVM> GetAddCourse(int id);
        Task AddCourse(AddCourseVM model);
        Task<DashboardVM> GetDashboardData(int teacherId);
        Task<EduGate.ViewModels.Shared.SettingsVM> GetSettings(int teacherId);
        Task<bool> UpdateProfile(int teacherId, EduGate.ViewModels.Shared.SettingsVM model);
        Task<bool> UpdatePassword(int teacherId, EduGate.ViewModels.Shared.SettingsVM model);
        Task<UploadMaterialVM> GetUploadMaterial(int courseId);
        Task UploadMaterial(UploadMaterialVM model);
        Task<ExamPageVM> GetAllExams(int teacherId);
        Task<AddExamVM> GetAddExam(int teacherId);
        Task AddExam(AddExamVM model);
        Task<EditExamVM?> GetEditExam(int examId);
        Task EditExam(EditExamVM model);
        Task DeleteExam(int examId);
        Task<EditLessonVM> GetUpdateLesson(int lessonid, int teacherid);
        Task UpdateLesson(EditLessonVM model);
        Task<EditQuizVM> GetUpdateQuiz(int quizId);
        Task UpdateQuiz(EditQuizVM model);
    }
}
