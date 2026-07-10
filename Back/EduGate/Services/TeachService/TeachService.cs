using AspNetCoreGeneratedDocument;
using EduGate.Data;
using EduGate.Enums;
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
                        Id = l.Id,
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
                        Id = q.Id,
                        Name = q.Name,
                        QusetionConut = q.Questions != null ? q.Questions.Count() : 0
                    }).ToList() ?? new List<QuizVM>(),
                    Assessments = c.Exams.Where(e => e.Type == Enums.ExamType.Assignment)
                    .Select(a => new AssessmentVM
                    {
                        Id = a.Id,
                        Name = a.Name,
                        DueDate = a.DueDate.HasValue ? a.DueDate.Value : default
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

           
            var subscription = await _context.Subscription
                .Include(s => s.package)
                .Where(s => s.Teacher_Id == teacherId && s.Status == Enums.SubscriptionStatus.Active)
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            
            if (subscription == null)
            {
                subscription = await _context.Subscription
                    .Include(s => s.package)
                    .Where(s => s.Teacher_Id == teacherId)
                    .OrderByDescending(s => s.Id)
                    .FirstOrDefaultAsync();
            }

            var maxStudents = subscription?.package?.Max_Students ?? 0;
            var remainingSeats = maxStudents - totalStudents;
            if (remainingSeats < 0) remainingSeats = 0;

            return new DashboardVM
            {
                TotalStudents = totalStudents,
                ActiveCourses = activeCourses,
                UpcomingExams = upcomingExams,
                RemainingSeats = remainingSeats,

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
        public async Task<EditLessonVM> GetUpdateLesson(int lessonid, int teacherid)
        {
            var teacher = await _context.Teacher.Where(t => t.Id == teacherid).FirstOrDefaultAsync();
            var lesson = await _context.Lesson.Where(l => l.Id == lessonid).FirstOrDefaultAsync();
            var data = new EditLessonVM 
            {
                TeacherName = teacher.First_Name + " " + teacher.Last_Name,
                Initials = $"{char.ToUpper(teacher.First_Name[0])}{char.ToUpper(teacher.Last_Name[0])}",
                LessonId = lessonid,
                LessonTitle = lesson.Name,
                VideoURL = lesson.Video_Url,
                CourseId = lesson.Course_Id
            };
            return data;
        }
        public async Task UpdateLesson(EditLessonVM model)
        {
            var lesson = await _context.Lesson.Where(l => l.Id == model.LessonId).FirstOrDefaultAsync();
            lesson.Name = model.LessonTitle;
            lesson.Video_Url = model.VideoURL;
            lesson.UpdatedAt = DateTime.Now;
            _context.Lesson.Update(lesson);
            await _context.SaveChangesAsync();
        }
        public async Task<EditQuizVM> GetUpdateQuiz(int quizId)
        {
            var data = await _context.Exam
                .Where(e => e.Id == quizId)
                .Select(x => new EditQuizVM
                {
                    TeacherName = x.course.teacher.First_Name + " " + x.course.teacher.Last_Name,
                    Initials = $"{char.ToUpper(x.course.teacher.First_Name[0])}{char.ToUpper(x.course.teacher.Last_Name[0])}",
                    QuizId = x.Id,
                    CourseId = x.Course_Id,
                    QuizTitle = x.Name,
                    Date = x.DueDate.HasValue ? DateOnly.FromDateTime(x.DueDate.Value) : default,
                    Time = x.DueDate.HasValue ? TimeOnly.FromDateTime(x.DueDate.Value) : default,
                    Duration = x.Duration ?? 0,
                    PassingScore = x.PassingPercentage,
                    TotalMarks = x.Total_Marks,
                    Questions = x.Questions
                        .Select(q => new EditQuestionVM
                        {
                            QuestionId = q.Id,
                            Text = q.Text,
                            Mark = q.Mark,
                            Choices = q.Choices
                                .Select(c => new EditChoiceVM
                                {
                                    ChoiceId = c.Id,
                                    Text = c.Text,
                                    IsCorrect = c.IsCorrect
                                }).ToList(),
                            CorrectChoiceId = q.Choices
                                .Where(c => c.IsCorrect)
                                .Select(c => c.Id)
                                .FirstOrDefault()
                        }).ToList()
                }).FirstOrDefaultAsync();
            return data;
        }
        private void UpdateQuizInfo(Exam quiz, EditQuizVM model)
        {
            quiz.Name = model.QuizTitle;
            quiz.Duration = model.Duration;
            quiz.PassingPercentage = model.PassingScore;
            quiz.Total_Marks = model.TotalMarks;
            quiz.StartDate = model.Date.ToDateTime(model.Time);
            quiz.UpdatedAt = DateTime.Now;
        }
        private void AddQuestion(Exam quiz, EditQuestionVM questionVm)
        {
            var question = new Question
            {
                Text = questionVm.Text,
                Mark = questionVm.Mark,
                Choices = new List<Choice>()
            };

            foreach (var choiceVm in questionVm.Choices)
            {
                question.Choices.Add(new Choice
                {
                    Text = choiceVm.Text,
                    IsCorrect = choiceVm.IsCorrect,
                });
            }

            quiz.Questions.Add(question);
        }
        private void UpdateChoice(Question question, EditChoiceVM choiceVm)
        {
            var dbChoice = question.Choices
                .FirstOrDefault(c => c.Id == choiceVm.ChoiceId);

            dbChoice.Text = choiceVm.Text;
            dbChoice.IsCorrect = choiceVm.IsCorrect;
        }
        private void UpdateQuestion(Exam quiz, EditQuestionVM questionVm)
        {
            var dbQuestion = quiz.Questions
                .FirstOrDefault(q => q.Id == questionVm.QuestionId);

            dbQuestion.Text = questionVm.Text;
            dbQuestion.Mark = questionVm.Mark;

            foreach (var choiceVm in questionVm.Choices)
            {
                UpdateChoice(dbQuestion, choiceVm);
            }
        }
        public async Task UpdateQuiz(EditQuizVM model)
        {
            var quiz = await _context.Exam
                .Include(e => e.Questions)
                .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(e => e.Id == model.QuizId);

            if (quiz == null)
                return;

            UpdateQuizInfo(quiz, model);
            foreach (var questionVm in model.Questions)
            {
                if (questionVm.QuestionId == 0)
                    AddQuestion(quiz, questionVm);
                else
                    UpdateQuestion(quiz, questionVm);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<EditAssessmentVM> GetUpdateAssessment(int assessmentId)
        {
            var data = await _context.Exam
                .Where(e => e.Id == assessmentId)
                .Select(x => new EditAssessmentVM
                {
                    TeacherName = x.course.teacher.First_Name + " " + x.course.teacher.Last_Name,
                    Initials = $"{char.ToUpper(x.course.teacher.First_Name[0])}{char.ToUpper(x.course.teacher.Last_Name[0])}",
                    AssessmentId = x.Id,
                    CourseId = x.Course_Id,
                    AssessmentTitle = x.Name,
                    Date = x.DueDate.HasValue ? DateOnly.FromDateTime(x.DueDate.Value) : default,
                    Time = x.DueDate.HasValue ? TimeOnly.FromDateTime(x.DueDate.Value) : default,
                    TotalPoints = x.Total_Marks,
                    PassingScore = x.PassingPercentage,
                    Instructions = x.Questions.FirstOrDefault().Text
                }).FirstOrDefaultAsync();
            return data;
        }
        public async Task UpdateAssessment(EditAssessmentVM model)
        {
            var assessment = await _context.Exam
                .Where(e => e.Id == model.AssessmentId)
                .Include(e => e.Questions)
                .FirstOrDefaultAsync();
            assessment.Name = model.AssessmentTitle;
            assessment.DueDate = model.Date.ToDateTime(model.Time);
            assessment.Total_Marks = model.TotalPoints;
            assessment.PassingPercentage = model.PassingScore;
            assessment.Questions.FirstOrDefault().Text = model.Instructions;
            assessment.Questions.FirstOrDefault().Mark = model.TotalPoints;
            assessment.UpdatedAt = DateTime.Now;
            _context.Exam.Update(assessment);
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
                .Where(e => e.Type == ExamType.Exam)
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
                .Include(e => e.course.teacher)
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null) return null;

            return new EditExamVM
            {
                TeacherName = exam.course.teacher.First_Name + " " + exam.course.teacher.Last_Name,
                Initials = $"{char.ToUpper(exam.course.teacher.First_Name[0])}{char.ToUpper(exam.course.teacher.Last_Name[0])}",
                ExamId = exam.Id,
                CourseId = exam.Course_Id,
                CourseName = exam.course.Name,
                ExamTitle = exam.Name,
                Date = DateOnly.FromDateTime(exam.StartDate),
                Time = TimeOnly.FromDateTime(exam.StartDate),
                Duration = exam.Duration ?? 0,
                PassingScore = exam.PassingPercentage,
                TotalMarks = exam.Total_Marks,
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
            exam.Total_Marks = model.TotalMarks;
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

            var examAttempts = await _context.ExamAttempt
                .Where(ea => ea.Exam_Id == examId)
                .ToListAsync();

            if (examAttempts.Any())
            {
                _context.ExamAttempt.RemoveRange(examAttempts);
            }

            foreach (var question in exam.Questions)
            {
                _context.Choice.RemoveRange(question.Choices);
            }
            _context.Question.RemoveRange(exam.Questions);

            _context.Exam.Remove(exam);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteLesson(int lessonId)
        {
            var lesson = await _context.Lesson
                .Include(l => l.Materials)
                .FirstOrDefaultAsync(l => l.Id == lessonId);

            if (lesson == null) return;

            if (lesson.Materials != null && lesson.Materials.Any())
                _context.Material.RemoveRange(lesson.Materials);

            _context.Lesson.Remove(lesson);
            await _context.SaveChangesAsync();
        }
        public async Task<GenerateStudentsVM> GetGenerateStudents(int teacherid)
        {
            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(t => t.Id == teacherid);

            var subscription = await _context.Subscription
                .Include(s => s.package)
                .FirstOrDefaultAsync(s => s.Teacher_Id == teacherid && s.Status == SubscriptionStatus.Active);

            var generatedaccounts = await _context.Account
                .CountAsync(a => a.Teacher_Id == teacherid);

            var data = new GenerateStudentsVM
            {
                TeacherName = teacher.First_Name + " " + teacher.Last_Name,
                Initials = $"{char.ToUpper(teacher.First_Name[0])}{char.ToUpper(teacher.Last_Name[0])}",
                TeacherId = teacherid,
                MaxStudents = subscription.package.Max_Students,
                RemainingSlots = subscription.package.Max_Students - generatedaccounts
            };

            data.Courses.Add(new SelectListItem 
            {
                Text = "All Courses",
                Value = "0"
            });

            data.Courses.AddRange(await _context.Course
                .Where(c => c.Teacher_Id == teacherid)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .ToListAsync()
            );

            return data;
        }
        private readonly Random _random = new();
        private async Task<string> GenerateUserName(GenerateStudentsVM model, int index)
        {
            if (model.NamingType == NamingType.FixedPrefix)
            {
                var count = await _context.Account
                    .CountAsync(a => a.User_Name.StartsWith(model.Prefix + "_"));

                return $"{model.Prefix}_{count + index}";
            }

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            while (true)
            {
                var suffix = new string(Enumerable.Repeat(chars, 5)
                    .Select(s => s[_random.Next(s.Length)])
                    .ToArray());

                var userName = $"student{suffix}";

                bool exists = await _context.Account
                    .AnyAsync(a => a.User_Name == userName);

                if (!exists)
                    return userName;
            }
        }
        private string GeneratePassword()
        {
            const string chars =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";

            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[_random.Next(s.Length)])
                .ToArray());
        }
        public async Task<GenerateStudentsVM> GenerateStudents(GenerateStudentsVM model)
        {
            var data = await GetGenerateStudents(model.TeacherId);
            model.TeacherName = data.TeacherName;
            model.Initials = data.Initials;
            model.TeacherId = data.TeacherId;
            model.Courses = data.Courses;
            model.RemainingSlots = data.RemainingSlots;
            model.MaxStudents = data.MaxStudents;
            if (model.NumberOfStudents > model.RemainingSlots)
            {
                throw new Exception("The number of students exceeds your remaining slots.");
            }
            if (model.NamingType == NamingType.FixedPrefix)
            {
                if (string.IsNullOrWhiteSpace(model.Prefix))
                {
                    throw new Exception("Prefix is required.");
                }
                bool exists = await _context.Account
                .AnyAsync(a => a.User_Name.StartsWith(model.Prefix + "_"));
                if (exists)
                {
                    throw new Exception("This prefix is already used.");
                }
            }
            List<Course> courses;
            if (model.CourseId == 0)
            {
                courses = await _context.Course
                    .Where(c => c.Teacher_Id == model.TeacherId)
                    .ToListAsync();
            }
            else
            {
                courses = await _context.Course
                    .Where(c => c.Id == model.CourseId)
                    .ToListAsync();
            }
            for (int i = 1; i <= model.NumberOfStudents; i++)
            {
                var userName = await GenerateUserName(model, i);
                var password = GeneratePassword();
                var student = new Student
                {
                    First_Name = "Student",
                    Last_Name = i.ToString(),
                    Gender = Gender.Male,
                    Phone = ""
                };
                var account = new Account
                {
                    User_Name = userName,
                    Password = password,
                    Status = AccountStatus.Active,
                    CreatedAt = DateTime.Now,
                    student = student,
                    Teacher_Id = model.TeacherId
                };
                _context.Account.Add(account);
                await _context.SaveChangesAsync();
                foreach (var course in courses)
                {
                    _context.Enrollment.Add(new Enrollment
                    {
                        Student_Id = student.Id,
                        Course_Id = course.Id
                    });
                }
                model.GeneratedStudents.Add(new GeneratedStudentVM
                {
                    Email = userName,
                    Password = password
                });
            }
            await _context.SaveChangesAsync();
            return model;
        }
        public async Task<UpgradeVM> GetUpgrade(int teacherid)
        {
            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(t => t.Id == teacherid);
            var data = new UpgradeVM 
            {
                TeacherName = teacher.First_Name + " " + teacher.Last_Name,
                Initials = $"{char.ToUpper(teacher.First_Name[0])}{char.ToUpper(teacher.Last_Name[0])}"
            };
            return data;
        }
    }
}
