using EduGate.Data;
using EduGate.Models;
using EduGate.ViewModels.Teacher;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                        QusetionConut = q.Questions != null ? q.Questions.Count() : 0
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
        public async Task<AddQuizVM> GetAddQuiz(int courseId)
        {
            var data = await _context.Course
                .Where(c => c.Id == courseId)
                .Select(x => new AddQuizVM
                {
                    CourseId = x.Id,
                    CourseName = x.Name,
                    TeacherName = x.teacher.First_Name + " " + x.teacher.Last_Name,
                    Initials = $"{char.ToUpper(x.teacher.First_Name[0])}{char.ToUpper(x.teacher.Last_Name[0])}"
                }).FirstOrDefaultAsync();

            return data;
        }
        public async Task AddQuiz(AddQuizVM model)
        {
            var quiz = new Exam
            {
                Name = model.QuizTitle,
                Course_Id = model.CourseId,
                StartDate = model.Date.ToDateTime(model.Time),
                CreatedAt = DateTime.Now,
                Duration = model.Duration,
                PassingPercentage = model.PassingScore,
                Type = Enums.ExamType.Quiz,
                Total_Marks = model.TotalMark,
                Questions = new List<Question>()
            };

            foreach (var q in model.Questions)
            {
                var question = new Question
                {
                    Text = q.Text,
                    Mark = q.Mark,
                    Choices = new List<Choice>()
                };

                foreach (var c in q.Choices)
                {
                    question.Choices.Add(new Choice
                    {
                        Text = c.Text,
                        IsCorrect = c.IsCorrect
                    });
                }

                quiz.Questions.Add(question);
            }

            _context.Exam.Add(quiz);
            await _context.SaveChangesAsync();
        }
        public async Task<AddAssessmentVM> GetAddAssessment(int courseId)
        {
            var data = await _context.Course
                .Where(c => c.Id == courseId)
                .Select(x => new AddAssessmentVM
                {
                    CourseId = x.Id,
                    CourseName = x.Name,
                    TeacherName = x.teacher.First_Name + " " + x.teacher.Last_Name,
                    Initials = $"{char.ToUpper(x.teacher.First_Name[0])}{char.ToUpper(x.teacher.Last_Name[0])}"
                }).FirstOrDefaultAsync();

            return data;
        }
        public async Task AddAssessment(AddAssessmentVM model)
        {
            var assessment = new Exam
            {
                Name = model.AssessmentTitle,
                CreatedAt = DateTime.Now,
                StartDate = DateTime.Now,
                DueDate = model.Date.ToDateTime(model.Time),
                Type = Enums.ExamType.Assignment,
                Total_Marks = model.TotalMark,
                PassingPercentage = model.PassingScore,
                Course_Id = model.CourseId
            };

            _context.Exam.Add(assessment);
            await _context.SaveChangesAsync();

            var question = new Question
            {
                Text = model.Instruction,
                Mark = model.TotalMark,
                Exam_Id = assessment.Id
            };

            _context.Question.Add(question);
            await _context.SaveChangesAsync();
        }
        public async Task<AddCourseVM> GetAddCourse(int id)
        {
            var data = await _context.Teacher
                .Where(t => t.Id == id)
                .Select(x => new AddCourseVM
                {
                    TeacherName = x.First_Name + " " + x.Last_Name,
                    Initials = $"{char.ToUpper(x.First_Name[0])}{char.ToUpper(x.Last_Name[0])}",
                    TeacherId = id
                }).FirstOrDefaultAsync();

            return data;
        }
        public async Task AddCourse(AddCourseVM model)
        {
            var course = new Course
            {
                Name = model.CourseName,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                Teacher_Id = model.TeacherId
            };

            _context.Course.Add(course);
            await _context.SaveChangesAsync();
        }
        public async Task<UploadMaterialVM> GetUploadMaterial(int courseId)
        {
            var data = await _context.Course
                .Where(c => c.Id == courseId)
                .Include(c => c.teacher)
                .Select(x => new UploadMaterialVM
                {
                    Lessons = x.Lessons.Select(l => new SelectListItem
                    {
                        Value = l.Id.ToString(),
                        Text = l.Name
                    }).ToList(),
                    TeacherName = x.teacher.First_Name + " " + x.teacher.Last_Name,
                    CourseId = x.Id,
                    CourseName = x.Name,
                    Initials = $"{char.ToUpper(x.teacher.First_Name[0])}{char.ToUpper(x.teacher.Last_Name[0])}"
                }).FirstOrDefaultAsync();

            return data;
        }
        public async Task UploadMaterial(UploadMaterialVM model)
        {
            var material = new LessonMaterial
            {
                Title = model.Title,
                Lesson_Id = model.LessonId,
                File_Type = model.File.ContentType,
                File_Size = (model.File.Length / (1024 * 1024)),
                File_Path = $"https://edugate.com/videos/{model.File.FileName}"
            };

            _context.Material.Add(material);
            await _context.SaveChangesAsync();
        }

        

    }
}
