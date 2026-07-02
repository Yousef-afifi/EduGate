using EduGate.Data;
using EduGate.Enums;
using EduGate.Models;
using EduGate.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduGate.Services.StuServices
{
    public class StuService : IStuService
    {
        private readonly AppDbContext _context;
        public StuService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CourseVM>> GetStudentCoursesAsync(int id)
        {
            var data = await _context.Enrollment
                .Where(s => s.Student_Id == id)
                .Select(s => s.course) 
                .Select(c => new CourseVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    TeacherName = c.teacher != null ? c.teacher.First_Name : "No Teacher",
                    LessonCount = c.Lessons != null ? c.Lessons.Count() : 0,
                    ExamCount = c.Exams != null ? c.Exams.Count() : 0
                })
                .ToListAsync();

            return data;
        }
        public async Task<CourseDetailsVM> GetCourseDeatailsAsync(int id)
        {
            var course = await _context.Course
                 .Include(c => c.teacher)
                 .Include(c => c.Lessons).ThenInclude(l => l.Materials)
                 .Include(c => c.Exams)
                 .ThenInclude(c => c.Questions)
                 .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return null;

            return new CourseDetailsVM
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                TeacherName = course.teacher != null ? course.teacher.First_Name : "No Teacher",
                ProgressPrecentage = 78,

                Lessons = course.Lessons?.Select(l => new LessonVM
                {
                    Id = l.Id,
                    Name = l.Name,
                    Video_Url = l.Video_Url,
                    Materials = l.Materials?.Select(m => new LessonMaterialVM
                    {
                        Id = m.Id,
                        FileName = m.File_Path,
                        FileSize = "1.2 MB",
                        UploadedDate = "Jan 10",
                        DownloadUrl = m.File_Path
                    }).ToList() ?? new List<LessonMaterialVM>(),
                }).ToList() ?? new List<LessonVM>(),

                Quizzes = course.Exams?.Where(e => e.Type == Enums.ExamType.Quiz)
                .Select(q => new QuizVM
                {
                    Id = q.Id,
                    Name = q.Name,
                    QuestionCount = q.Questions != null ? q.Questions.Count() : 0,
                    score = "", 
                    Status = ""

                }).ToList() ?? new List<QuizVM>(),

                Assessments = course.Exams?.Where(e => e.Type == Enums.ExamType.Assignment || e.Type == ExamType.Exam)
                .Select(a => new AssessmentVM
                {
                    Id = a.Id,
                    Name = a.Name,
                    score = "",
                    Status = "",
                    DueDate = a.Duration.HasValue ? $"{a.Duration.Value} Mins" : "No Limit"

                }).ToList() ?? new List<AssessmentVM>(),
            };
        }   
    }
}
