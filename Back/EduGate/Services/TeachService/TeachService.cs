using EduGate.Data;
using EduGate.ViewModels.Teacher;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace EduGate.Services.TeachService
{
    public class TeachService : ITeachService
    {
        private readonly AppDbContext _context;
        public TeachService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CourseVM>> GetAllCoursesAsync(int id)
        {
            var data = await _context.Course
                .Where(t => t.Teacher_Id == id)
                .Select(c => new CourseVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    TeacherName = c.teacher.First_Name,
                    LessonCount = c.Lessons != null ? c.Lessons.Count() : 0,
                })
                .ToListAsync();

            return data;
        }
        public async Task<CourseDetailsVM> GetCourseDetailsAsync(int courseId)
        {
            var data = await _context.Course
                .Where(c => c.Id == courseId)
                .Select(c => new CourseDetailsVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    LessonCount = c.Lessons != null ? c.Lessons.Count() : 0,
                    Lessons = c.Lessons.Select(l => new LessonVM
                    {
                        Name = l.Name,
                        Materials = l.Materials.Select(m => new LessonMaterialVM
                        {
                            Name = m.Title,
                            Url = m.File_Path,
                            File_Size = m.File_Size
                        }).ToList() ?? new List<LessonMaterialVM>()
                    }).ToList() ?? new List<LessonVM>(),
                    Quizzes = c.Exams.Where(e => e.Type == Enums.ExamType.Quiz)
                    .Select(q => new QuizVM
                    {
                        Name = q.Name,
                        QustionConut = q.Questions != null ? q.Questions.Count() : 0
                    }).ToList() ?? new List<QuizVM>(),
                    Assessments = c.Exams.Where(e => e.Type == Enums.ExamType.Assignment)
                    .Select(a => new AssessmentVM
                    {
                        Name = a.Name,
                        DueDate = a.CreatedAt.AddDays(7)
                    }).ToList() ?? new List<AssessmentVM>()
                }).FirstOrDefaultAsync();
            return data;
        }
    }
}
