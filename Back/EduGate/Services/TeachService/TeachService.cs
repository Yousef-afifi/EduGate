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

        public async Task<DashboardVM> GetDashboardData(int teacherId)
        {
            
            var teacher = await _context.Teacher.FirstOrDefaultAsync(t => t.Id == teacherId);

            
            var totalStudents = await _context.Account
                .Where(a => a.Teacher_Id == teacherId && a.Student_Id != 0)
                .CountAsync();

            
            var activeCourses = await _context.Course
                .Where(c => c.Teacher_Id == teacherId)
                .CountAsync();

           
            var upcomingExams = await _context.Exam
                .Where(e => e.course.Teacher_Id == teacherId && e.StartDate > DateTime.Now)
                .CountAsync();

            
            var recentCourses = await _context.Course
                .Where(c => c.Teacher_Id == teacherId)
                .OrderByDescending(c => c.CreatedAt)
                .Take(4)
                .Select(c => new EduGate.ViewModels.Teacher.RecentCourseVM
                {
                    Name = c.Name,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            

            
            return new DashboardVM
            {
                TotalStudents = totalStudents,
                ActiveCourses = activeCourses,
                UpcomingExams = upcomingExams,

                TeacherName = teacher.First_Name + " " + teacher.Last_Name,
                Initials = $"{char.ToUpper(teacher.First_Name[0])}{char.ToUpper(teacher.Last_Name[0])}",

                
                RecentCourses = recentCourses,
                
            };
        }
        public async Task<EduGate.ViewModels.Shared.SettingsVM> GetSettings(int teacherId)
        {
            var teacher = await _context.Teacher.FirstOrDefaultAsync(t => t.Id == teacherId);
            return new EduGate.ViewModels.Shared.SettingsVM
            {
                FirstName = teacher.First_Name,
                LastName = teacher.Last_Name,
                Email = teacher.Email,
                TeacherName = teacher.First_Name + " " + teacher.Last_Name,
                Initials = $"{char.ToUpper(teacher.First_Name[0])}{char.ToUpper(teacher.Last_Name[0])}"
            };
        }

        public async Task<bool> UpdateProfile(int teacherId, EduGate.ViewModels.Shared.SettingsVM model)
        {
            var teacher = await _context.Teacher.FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher == null) return false;

            teacher.First_Name = model.FirstName;
            teacher.Last_Name = model.LastName;
            teacher.Email = model.Email;

            _context.Teacher.Update(teacher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePassword(int teacherId, EduGate.ViewModels.Shared.SettingsVM model)
        {
            var teacher = await _context.Teacher.FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher == null) return false;

            if (teacher.Password == model.CurrentPassword)
            {
                teacher.Password = model.NewPassword;
                _context.Teacher.Update(teacher);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
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

        //--------------------------------------------------------------------------------
        public async Task<ExamPageVM> GetAllExams(int teacherId)
        {
            var teacher = await _context.Teacher.FirstOrDefaultAsync(t => t.Id == teacherId);

            var exams = await _context.Exam
                .Include(e => e.course)
                .Include(e => e.Questions)
                .Where(e => e.course.Teacher_Id == teacherId)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();

            var courseIds = exams.Select(e => e.Course_Id).Distinct().ToList();
            var enrolledCounts = await _context.Enrollment
                .Where(en => courseIds.Contains(en.Course_Id))
                .GroupBy(en => en.Course_Id)
                .Select(g => new { CourseId = g.Key, Count = g.Count() })
                .ToListAsync();

            return new ExamPageVM
            {
                TeacherName = teacher != null ? teacher.First_Name + " " + teacher.Last_Name : "",
                Initials = teacher != null ? $"{teacher.First_Name?[0]}{teacher.Last_Name?[0]}".ToUpper() : "??",
                Exams = exams.Select(e => new ExamListItemVM
                {
                    Id = e.Id,
                    Name = e.Name,
                    Type = e.Type,
                    CourseName = e.course.Name,
                    StartDate = e.StartDate,
                    Duration = e.Duration,
                    QuestionCount = e.Questions?.Count ?? 0,
                    EnrolledCount = enrolledCounts.FirstOrDefault(x => x.CourseId == e.Course_Id)?.Count ?? 0
                }).ToList()
            };
        }

        public async Task<AddExamVM> GetAddExam(int teacherId)
        {
            var teacher = await _context.Teacher.FirstOrDefaultAsync(t => t.Id == teacherId);

            var courses = await _context.Course
                .Where(c => c.Teacher_Id == teacherId)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            return new AddExamVM
            {
                TeacherName = teacher != null ? teacher.First_Name + " " + teacher.Last_Name : "",
                Initials = teacher != null ? $"{teacher.First_Name?[0]}{teacher.Last_Name?[0]}".ToUpper() : "??",
                Courses = courses
            };
        }

        public async Task AddExam(AddExamVM model)
        {
            var exam = new Exam
            {
                Name = model.ExamTitle,
                Course_Id = model.CourseId,
                StartDate = model.Date.ToDateTime(model.Time),
                CreatedAt = DateTime.Now,
                Duration = model.Duration,
                PassingPercentage = model.PassingScore,
                Type = Enums.ExamType.Exam,
                Total_Marks = model.Questions.Sum(q => q.Mark),   
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

                exam.Questions.Add(question);
            }

            _context.Exam.Add(exam);
            await _context.SaveChangesAsync();
        }

        public async Task<EditExamVM?> GetEditExam(int examId)
        {
            var exam = await _context.Exam
                .Include(e => e.course)
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null) return null;

            return new EditExamVM
            {
                ExamId = exam.Id,
                CourseId = exam.Course_Id,
                CourseName = exam.course.Name,
                ExamTitle = exam.Name,
                Date = DateOnly.FromDateTime(exam.StartDate),
                Time = TimeOnly.FromDateTime(exam.StartDate),
                Duration = exam.Duration ?? 0,
                PassingScore = exam.PassingPercentage,
                Questions = exam.Questions?.Select(q => new QuestionVM
                {
                    Text = q.Text,
                    Mark = q.Mark,
                    Choices = q.Choices.Select(c => new ChoiceVM
                    {
                        Text = c.Text,
                        IsCorrect = c.IsCorrect
                    }).ToList()
                }).ToList() ?? new()
            };
        }

        public async Task EditExam(EditExamVM model)
        {
            var exam = await _context.Exam
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(e => e.Id == model.ExamId);

            if (exam == null) return;

            exam.Name = model.ExamTitle;
            exam.StartDate = model.Date.ToDateTime(model.Time);
            exam.Duration = model.Duration;
            exam.Total_Marks = model.Questions.Sum(q => q.Mark);
            exam.PassingPercentage = model.PassingScore;
            exam.UpdatedAt = DateTime.Now;

            if (exam.Questions != null)
                _context.Question.RemoveRange(exam.Questions);

            exam.Questions = model.Questions.Select(q => new Question
            {
                Text = q.Text,
                Mark = q.Mark,
                Choices = q.Choices.Select(c => new Choice
                {
                    Text = c.Text,
                    IsCorrect = c.IsCorrect
                }).ToList()
            }).ToList();

            await _context.SaveChangesAsync();
        }

        public async Task DeleteExam(int examId)
        {
            var exam = await _context.Exam
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null) return;

            foreach (var question in exam.Questions)
            {
                _context.Choice.RemoveRange(question.Choices);
            }
            _context.Question.RemoveRange(exam.Questions);

            _context.Exam.Remove(exam);
            await _context.SaveChangesAsync();
        }


    }
}
