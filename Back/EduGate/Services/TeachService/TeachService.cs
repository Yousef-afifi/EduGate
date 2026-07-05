using EduGate.Data;
using EduGate.Models;
using EduGate.ViewModels.Teacher;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
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
        public async Task<CoursePageVM> GetAllCoursesAsync(int id)
        {
            var courses = await _context.Course
                .Where(t => t.Teacher_Id == id)
                .Select(c => new CourseVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    TeacherName = c.teacher.First_Name,
                    LessonCount = c.Lessons != null ? c.Lessons.Count() : 0,
                    StudentConut = _context.Enrollment.Where(e => e.Course_Id == c.Id).Count()
                })
                .ToListAsync();

            var data = await _context.Teacher
                .Where(t => t.Id == id)
                .Select(x => new CoursePageVM
                {
                    TeacherName = x.First_Name + " " + x.Last_Name,
                    Initials = $"{char.ToUpper(x.First_Name[0])}{char.ToUpper(x.Last_Name[0])}",
                    Courses = courses
                }).FirstOrDefaultAsync();

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
                    TeacherName = c.teacher.First_Name + " " + c.teacher.Last_Name,
                    Initials = $"{char.ToUpper(c.teacher.First_Name[0])}{char.ToUpper(c.teacher.Last_Name[0])}",
                    LessonCount = c.Lessons != null ? c.Lessons.Count() : 0,
                    StudentConut = _context.Enrollment.Where(e => e.Course_Id == c.Id).Count(),
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
        public async Task<StudentPageVM> GetAllStudentsAsync(int id)
        {
            var students = await _context.Account
                .Where(a => a.Teacher_Id == id)
                .Include(a => a.student)
                .Select(s => new StudentVM 
                {
                    StudentName = s.student.First_Name + " " + s.student.Last_Name,
                    Initials = $"{char.ToUpper(s.student.First_Name[0])}{char.ToUpper(s.student.Last_Name[0])}",
                    StudentEmail = s.User_Name,
                    Status = s.Status,
                    Joined = DateOnly.FromDateTime(s.CreatedAt),
                    ECourseCount = _context.Enrollment.Where(e => e.Student_Id == s.Student_Id).Count()
                })
                .ToListAsync();

            var data = await _context.Teacher
                .Where(t => t.Id == id)
                .Select(x => new StudentPageVM
                {
                    TeacherName = x.First_Name + " " + x.Last_Name,
                    Initials = $"{char.ToUpper(x.First_Name[0])}{char.ToUpper(x.Last_Name[0])}",
                    Students = students
                }).FirstOrDefaultAsync();

            return data;
        }
        public async Task<AddLessonVM> GetAddLesson(int Id)
        {
            var data = await _context.Course
                .Where(c => c.Id == Id)
                .Include(c => c.teacher)
                .Select(x => new AddLessonVM
                {
                    TeacherName = x.teacher.First_Name + " " + x.teacher.Last_Name,
                    Initials = $"{char.ToUpper(x.teacher.First_Name[0])}{char.ToUpper(x.teacher.Last_Name[0])}",
                    CourseName = x.Name,
                    CourseId = x.Id
                }).FirstOrDefaultAsync();

            return data;
        }
        public async Task AddLesson(AddLessonVM model)
        {
            var lesson = new Lesson
            {
                Name = model.LessonTitle,
                Video_Url = model.VideoUrl,
                Course_Id = model.CourseId,
                CreatedAt = DateTime.Now
            };
            _context.Lesson.Add(lesson);
            await _context.SaveChangesAsync();
        }
    }
}
