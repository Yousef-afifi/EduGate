using EduGate.Data;
using EduGate.Enums;
using EduGate.Models;
using EduGate.ViewModels.Student;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.VisualBasic;

namespace EduGate.Services.StuServices
{
    public class StuService : IStuService
    {
        private readonly AppDbContext _context;
        public StuService(AppDbContext context)
        {
            _context = context;
        }
        //-----------------Dashboard---------------------


        public async Task<DashboardVM> GetStudentOverview(int studentId)
        {
            var AllExamList = await GetExamInfo(studentId);
            var completedexam = await GetCompletedExams(studentId);
            var upcoming = await GetUpcomingExams(studentId);
            double avg = 0;
            foreach (var exam in completedexam) 
            {
                avg += exam.Score;

            }
            avg = completedexam.Count > 0 ? Math.Round(avg / completedexam.Count) : 0;

            var Student = await _context.Student
               .Where(s => s.Id == studentId)
               .FirstOrDefaultAsync();

            return new DashboardVM
            {
                initials = Student != null ? $"{Student.First_Name?[0]}{Student.Last_Name?[0]}".ToUpper() : "??",
                StudentName = Student != null ? Student.First_Name + " " + Student.Last_Name : "No Student",
                UpcomingExamsCount = upcoming.Count,
                CompletedExamsCount = completedexam.Count,
                ExamsAvgScore=avg,
                EnrolledCoursesCount = await _context.Enrollment.CountAsync(s => s.Student_Id == studentId),
                EnrolledCourses = await GetAllCoursesEnrolled(studentId),
                RecentGrades = await GetQuiz_AssesmentGrades(studentId),
                ExamsSchedule = AllExamList
            };

        }

        public async Task<List<CoursesVM>> GetAllCoursesEnrolled(int studentId)
        {
            var courses = await _context.Enrollment
                .Where(s => s.Student_Id == studentId)
                .Select(s => s.course)
                .Select(c => new CoursesVM
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            return courses;
        }

        public async Task<List<RecentGradesVM>> GetQuiz_AssesmentGrades(int studentId)
        {

            var studentAttempts = await _context.ExamAttempt
            .Include(a => a.exam)
            .ThenInclude(e => e.course)
            .Where(a => a.Student_Id == studentId && a.IsCompleted)
            .OrderByDescending(a => a.SubmittedAt)
            .ToListAsync();

            var recentGrades = studentAttempts.Select(a => new RecentGradesVM
            {
                Id = a.Id,
                AssessmentName = a.exam?.Name,
                CourseName = a.exam?.course?.Name,
                StudentGrade = a.Score,
                TotalMark = a.exam != null ? a.exam.Total_Marks : 0,

            }).ToList();

            return recentGrades;
        }
        //-----------------Courses-----------------------
        public async Task<coursepageVM> GetStudentCoursesAsync(int id)
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

            var Student = await _context.Student
               .Where(s => s.Id == id)
               .FirstOrDefaultAsync();


            var result = new coursepageVM
            {
                initials = Student != null ? $"{Student.First_Name?[0]}{Student.Last_Name?[0]}".ToUpper() : "??",
                StudentName = Student != null ? Student.First_Name + " " + Student.Last_Name : "No Student",
                courses = data,

            };


            return result;
        }
        public async Task<CourseDetailsVM> GetCourseDeatailsAsync(int studentId, int courseId)
        {

            var studentAttempts = await _context.ExamAttempt
                .Where(a => a.Student_Id == studentId)
                .ToListAsync();


            var Student = await _context.Student
               .Where(s => s.Id == studentId)
               .FirstOrDefaultAsync();


            var enrollment = await _context.Enrollment
                .Include(s => s.course).ThenInclude(c => c.teacher)
                .Include(s => s.course).ThenInclude(c => c.Lessons).ThenInclude(l => l.Materials)
                .Include(s => s.course).ThenInclude(c => c.Exams).ThenInclude(e => e.Questions)
                .FirstOrDefaultAsync(s => s.Student_Id == studentId && s.Course_Id == courseId);

            if (enrollment == null || enrollment.course == null)
                return null;

            var course = enrollment.course;

            return new CourseDetailsVM
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                TeacherName = course.teacher != null ? course.teacher.First_Name : "No Teacher",
                StudentName = Student != null ? Student.First_Name + " " + Student.Last_Name : "No Student",
                initials = Student != null ? $"{Student.First_Name?[0]}{Student.Last_Name?[0]}".ToUpper() : "??",

                Lessons = course.Lessons?.Select(l => new LessonVM
                {
                    Id = l.Id,
                    Name = l.Name,
                    Video_Url = l.Video_Url,
                    Materials = l.Materials?.Select(m => new LessonMaterialVM
                    {
                        Id = m.Id,
                        FileName = m.Title,
                        FileSize = m.File_Size,
                        DownloadUrl = m.File_Path
                    }).ToList() ?? new List<LessonMaterialVM>(),
                }).ToList() ?? new List<LessonVM>(),

                Quizzes = course.Exams?.Where(e => e.Type == Enums.ExamType.Quiz)
                .Select(q =>
                {
                    var attempt = studentAttempts.FirstOrDefault(a => a.Exam_Id == q.Id);

                    string status = "Not Started";
                    if (attempt != null)
                    {
                        status = attempt.IsCompleted ? "Completed" : "In Progress";
                    }

                    return new QuizVM
                    {
                        Id = q.Id,
                        Name = q.Name,
                        QuestionCount = q.Questions != null ? q.Questions.Count() : 0,
                        score = attempt != null && attempt.IsCompleted ? $"{attempt.Score}" : "--",
                        Status = status
                    };
                }).ToList() ?? new List<QuizVM>(),

                Assessments = course.Exams?.Where(e => e.Type == Enums.ExamType.Assignment)
                .Select(a =>
                {

                    var attempt = studentAttempts.FirstOrDefault(at => at.Exam_Id == a.Id);

                    string status = "Not Submitted";
                    if (attempt != null)
                    {
                        status = attempt.IsCompleted ? "Submitted" : "Incomplete / Draft";
                    }

                    return new AssessmentVM
                    {
                        Id = a.Id,
                        Name = a.Name,
                        score = attempt != null && attempt.IsCompleted ? $"{attempt.Score}/100" : "--",
                        Status = status,
                        DueDate = a.Duration.HasValue ? $"{a.Duration.Value} Mins" : "No Limit"
                    };
                }).ToList() ?? new List<AssessmentVM>(),
            };
        }

        public async Task<TakeExamVM?> TakeQuiz(int studentId, int QuizId)
        {
            var quiz = await _context.Exam
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(e => e.Id == QuizId);


            var Student = await _context.Student
               .Where(s => s.Id == studentId)
               .FirstOrDefaultAsync();


            if (quiz == null)
                return null;

            if (quiz.Type != ExamType.Quiz)
                return null;



            var isEnrolled = await _context.Enrollment.AnyAsync(e =>
                e.Student_Id == studentId &&
                e.Course_Id == quiz.Course_Id);

            if (!isEnrolled)
                return null;

            var attempt = await _context.ExamAttempt
                .FirstOrDefaultAsync(a =>
                    a.Student_Id == studentId &&
                    a.Exam_Id == QuizId);

            if (attempt != null && attempt.IsCompleted)
                return null;

            if (attempt == null)
            {
                attempt = new ExamAttempt
                {
                    Student_Id = studentId,
                    Exam_Id = QuizId,
                    StartedAt = DateTime.Now,
                    IsCompleted = false
                };

                _context.ExamAttempt.Add(attempt);
                await _context.SaveChangesAsync();
            }

            int durationInMinutes = quiz.Duration ?? 60;
            if (durationInMinutes <= 0) durationInMinutes = 60;


            DateTime endTime = attempt.StartedAt.Value.AddMinutes(durationInMinutes);

            int remainingSeconds = (int)(endTime - DateTime.Now).TotalSeconds;

            if (remainingSeconds <= 0 && (DateTime.Now - attempt.StartedAt.Value).TotalSeconds > 5)
            {
                attempt.IsCompleted = true;
                await _context.SaveChangesAsync();
                return null;
            }

            return new TakeExamVM
            {
                StudentName = Student != null ? Student.First_Name + " " + Student.Last_Name : "No Student",
                initials = Student != null ? $"{Student.First_Name?[0]}{Student.Last_Name?[0]}".ToUpper() : "??",
                ExamId = quiz.Id,
                ExamName = quiz.Name,
                Duration = quiz.Duration ?? 0,
                TotalMarks = quiz.Total_Marks,
                PassingPercentage = quiz.PassingPercentage,
                RemainingSeconds = remainingSeconds,
                Questions = quiz.Questions.Select(q => new QuestionVM
                {
                    Id = q.Id,
                    Text = q.Text,
                    Mark = q.Mark,

                    Choices = q.Choices.Select(c => new ChoiceVM
                    {
                        Id = c.Id,
                        Text = c.Text
                    }).ToList()

                }).ToList()
            };

        }

        public async Task<int> SubmitQuiz(int studentId, SubmitExamVM model)
        {
            var attempt = await _context.ExamAttempt
                .FirstOrDefaultAsync(a =>
                    a.Student_Id == studentId &&
                    a.Exam_Id == model.ExamId &&
                    !a.IsCompleted);

            if (attempt == null)
                return 0;

            int score = 0;

            foreach (var answer in model.Answers)
            {
                var choice = await _context.Choice
                    .FirstOrDefaultAsync(c =>
                        c.Id == answer.SelectedChoiceId);

                if (choice != null && choice.IsCorrect)
                {
                    var question = await _context.Question
                        .FirstOrDefaultAsync(q => q.Id == answer.QuestionId);

                    if (question != null)
                        score += question.Mark;
                }
            }

            attempt.Score = score;
            attempt.IsCompleted = true;
            attempt.SubmittedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            var exam = await _context.Exam.FirstOrDefaultAsync(e => e.Id == model.ExamId);

            return exam?.Course_Id ?? 0;
        }
        //---------------------------------------------------------------------------
        public async Task<StudentExamsVM> GetStudentExams(int studentId)
        {
            var Student = await _context.Student
               .Where(s => s.Id == studentId)
               .FirstOrDefaultAsync();

            return new StudentExamsVM
            {
                initials = Student != null ? $"{Student.First_Name?[0]}{Student.Last_Name?[0]}".ToUpper() : "??",
                StudentName = Student != null ? Student.First_Name + " " + Student.Last_Name : "No Student",
                UpcomingExams = await GetUpcomingExams(studentId),
                CompletedExams = await GetCompletedExams(studentId)
            };
        }

        public async Task<List<UpcomingExamVM>> GetUpcomingExams(int studentId)
        {
            var enrollments = await _context.Enrollment
                .Where(e => e.Student_Id == studentId)
                .Include(e => e.course)
                    .ThenInclude(c => c.Exams)
                        .ThenInclude(e => e.Questions)
                .ToListAsync();

            var exams = enrollments
                .SelectMany(e => e.course.Exams)
                .Where(e => e.Type == ExamType.Exam)
                .ToList();

            var completedExamIds = await _context.ExamAttempt
                .Where(a => a.Student_Id == studentId && a.IsCompleted)
                .Select(a => a.Exam_Id)
                .ToListAsync();

            return exams
                .Where(e => !completedExamIds.Contains(e.Id))
                .Select(e => new UpcomingExamVM
                {
                    Id = e.Id,
                    Name = e.Name,
                    Duration = e.Duration ?? 0,
                    TotalMarks = e.Total_Marks,
                    QuestionCount = e.Questions.Count,
                    PassingPercentage = e.PassingPercentage,
                    StartDate = e.StartDate
                })
                .ToList();
        }

        public async Task<List<CompletedExamVM>> GetCompletedExams(int studentId)
        {
            var enrollments = await _context.Enrollment
                .Where(e => e.Student_Id == studentId)
                .Include(e => e.course)
                    .ThenInclude(c => c.Exams)
                        .ThenInclude(e => e.Questions)
                .ToListAsync();

            var exams = enrollments
                .SelectMany(e => e.course.Exams)
                .Where(e => e.Type == ExamType.Exam)
                .ToList();

            var attempts = await _context.ExamAttempt
                .Where(a => a.Student_Id == studentId && a.IsCompleted)
                .ToListAsync();

            return exams
                .Where(e => attempts.Any(a => a.Exam_Id == e.Id))
                .Select(e => new CompletedExamVM
                {
                    Id = e.Id,
                    Name = e.Name,
                    Duration = e.Duration ?? 0,
                    TotalMarks = e.Total_Marks,
                    QuestionCount = e.Questions.Count,
                    Score = attempts.First(a => a.Exam_Id == e.Id).Score,
                    SubmittedAt = attempts.First(a => a.Exam_Id == e.Id).SubmittedAt ?? DateTime.MinValue
                })
                .ToList();
        }

        public async Task<TakeExamVM?> StartExam(int studentId, int examId)
        {
            var exam = await _context.Exam
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(e => e.Id == examId);

            var Student = await _context.Student
                .Where(s => s.Id == studentId)
                .FirstOrDefaultAsync();


            if (exam == null)
                return null;

            if (exam.Type != ExamType.Exam)
                return null;


            if (DateTime.Now < exam.StartDate)
            {
                return null;
            }

            var isEnrolled = await _context.Enrollment.AnyAsync(e =>
                e.Student_Id == studentId &&
                e.Course_Id == exam.Course_Id);

            if (!isEnrolled)
                return null;

            var attempt = await _context.ExamAttempt
                .FirstOrDefaultAsync(a =>
                    a.Student_Id == studentId &&
                    a.Exam_Id == examId);

            if (attempt != null && attempt.IsCompleted)
                return null;

            if (attempt == null)
            {
                attempt = new ExamAttempt
                {
                    Student_Id = studentId,
                    Exam_Id = examId,
                    StartedAt = DateTime.Now,
                    IsCompleted = false
                };

                _context.ExamAttempt.Add(attempt);
                await _context.SaveChangesAsync();
            }
           
            int durationInMinutes = exam.Duration ?? 60;
            if (durationInMinutes <= 0) durationInMinutes = 60; 

            DateTime endTime = attempt.StartedAt.Value.AddMinutes(durationInMinutes);

            int remainingSeconds = (int)(endTime - DateTime.Now).TotalSeconds;
            if (remainingSeconds <= 0 && (DateTime.Now - attempt.StartedAt.Value).TotalSeconds > 5)
            {
                attempt.IsCompleted = true;
                await _context.SaveChangesAsync();
                return null;
            }

            return new TakeExamVM
            {
                StudentName = Student != null ? Student.First_Name + " " + Student.Last_Name : "No Student",
                initials = Student != null ? $"{Student.First_Name?[0]}{Student.Last_Name?[0]}".ToUpper() : "??",
                ExamId = exam.Id,
                ExamName = exam.Name,
                Duration = exam.Duration ?? 0,
                TotalMarks = exam.Total_Marks,
                PassingPercentage = exam.PassingPercentage,
                RemainingSeconds = remainingSeconds,
                Questions = exam.Questions.Select(q => new QuestionVM
                {
                    Id = q.Id,
                    Text = q.Text,
                    Mark = q.Mark,

                    Choices = q.Choices.Select(c => new ChoiceVM
                    {
                        Id = c.Id,
                        Text = c.Text
                    }).ToList()

                }).ToList()
            };

        }
        public async Task SubmitExam(int studentId, SubmitExamVM model)
        {
            var attempt = await _context.ExamAttempt
                .FirstOrDefaultAsync(a =>
                    a.Student_Id == studentId &&
                    a.Exam_Id == model.ExamId &&
                    !a.IsCompleted);

            if (attempt == null)
                return;

            int score = 0;

            foreach (var answer in model.Answers)
            {
                var choice = await _context.Choice
                    .FirstOrDefaultAsync(c =>
                        c.Id == answer.SelectedChoiceId);

                if (choice != null && choice.IsCorrect)
                {
                    var question = await _context.Question
                        .FirstOrDefaultAsync(q => q.Id == answer.QuestionId);

                    if (question != null)
                        score += question.Mark;
                }
            }

            attempt.Score = score;
            attempt.IsCompleted = true;
            attempt.SubmittedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<ExamScheduleVM> GetExamSchedule(int studentId)
        {
            var Student = await _context.Student
                .Where(s => s.Id == studentId)
                .FirstOrDefaultAsync();

            return new ExamScheduleVM
            {
                StudentName = Student != null ? Student.First_Name + " " + Student.Last_Name : "No Student",
                initials = Student != null ? $"{Student.First_Name?[0]}{Student.Last_Name?[0]}".ToUpper() : "??",
                Examinfo = await GetExamInfo(studentId),
            };
            
        }



        public async Task<List<ExamInfoVM>> GetExamInfo(int studentId)
        {
            var enrollments = await _context.Enrollment
                .Where(e => e.Student_Id == studentId)
                .Include(e => e.course)
                    .ThenInclude(c => c.Exams)
                    .ToListAsync();

            var exams = enrollments
                .SelectMany(e => e.course.Exams)
                .Where(e => e.StartDate >= DateTime.Now)
                .ToList();

            var completedExamIds = await _context.ExamAttempt
                .Where(s => s.Student_Id == studentId && s.IsCompleted)
                .Select(s => s.Exam_Id)
                .ToListAsync();

            return exams
             .Where(e => !completedExamIds.Contains(e.Id))
             .OrderBy(e => e.StartDate)
             .Select(e => new ExamInfoVM
             {
                 Id = e.Id,
                 Name = e.Name,
                 Type = e.Type,
                 Duration = e.Duration,
                 StartDate = e.StartDate,

                 CourseName = e.course.Name
             })
             .ToList();
        }

        public async Task<EduGate.ViewModels.Shared.SettingsVM> GetSettings(int studentId)
        {
            var account = await _context.Account.Include(a => a.student).FirstOrDefaultAsync(a => a.Student_Id == studentId);
            if (account == null || account.student == null) return new EduGate.ViewModels.Shared.SettingsVM();

            return new EduGate.ViewModels.Shared.SettingsVM
            {
                FirstName = account.student.First_Name,
                LastName = account.student.Last_Name,
                Email = account.User_Name,
                TeacherName = account.student.First_Name + " " + account.student.Last_Name,
                Initials = $"{char.ToUpper(account.student.First_Name[0])}{char.ToUpper(account.student.Last_Name[0])}"
            };
        }

        public async Task<bool> UpdateProfile(int studentId, EduGate.ViewModels.Shared.SettingsVM model)
        {
            var account = await _context.Account.Include(a => a.student).FirstOrDefaultAsync(a => a.Student_Id == studentId);
            if (account == null || account.student == null) return false;

            account.student.First_Name = model.FirstName;
            account.student.Last_Name = model.LastName;
            account.User_Name = model.Email;

            _context.Account.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePassword(int studentId, EduGate.ViewModels.Shared.SettingsVM model)
        {
            var account = await _context.Account.FirstOrDefaultAsync(a => a.Student_Id == studentId);
            if (account == null) return false;

            if (account.Password == model.CurrentPassword)
            {
                account.Password = model.NewPassword;
                _context.Account.Update(account);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //---------------------------------------------------------------------------
        public async Task<SubmitAssessmentVM> GetSubmitAssessment(int ExamId , int StudentId)
        {
            var student = await _context.Student
                .Where(s => s.Id == StudentId)
                .FirstOrDefaultAsync();

            var question = await _context.Question
                .Where(q => q.Exam_Id == ExamId)
                .FirstOrDefaultAsync();

            if (student == null)
            {
                throw new Exception("Student Not Found");
            }

            if (question == null)
            {
                throw new Exception("Question Not Found");
            }

            var data = await _context.Exam
                .Where(e => e.Id == ExamId)
                .Include(e => e.course)
                .Select(x => new SubmitAssessmentVM
                {
                    AssessmentId = x.Id,
                    AssessmentTitle = x.Name,
                    QuestionId = question.Id,
                    Instructions = question.Text,
                    StudentId = StudentId,
                    StudentName = student.First_Name + " " + student.Last_Name,
                    Initials = $"{char.ToUpper(student.First_Name[0])}{char.ToUpper(student.Last_Name[0])}",
                    CourseId = x.Course_Id,
                    TotalMark = x.Total_Marks,
                    DueDate = x.DueDate
                }).FirstOrDefaultAsync();

            return data;
        }
        public async Task SubmitAssessment(SubmitAssessmentVM model)
        {
            var submit = new StudentAnswer 
            {
                AnswerdAt = DateTime.Now,
                Student_Id = model.StudentId,
                Question_Id = model.QuestionId,
                File_Name = model.File.FileName,
                File_Path = $"https://edugate.com/videos/{model.File.FileName}",
                File_Type = model.File.ContentType,
                File_Size = model.File.Length,
            };

            _context.StudentAnswer.Add(submit);
            await _context.SaveChangesAsync();

            var attempt = new ExamAttempt
            {
                IsCompleted = true,
                Student_Id = model.StudentId,
                Exam_Id = model.AssessmentId,
                SubmittedAt = DateTime.Now,
                Score = model.TotalMark ?? 0
            };

            _context.ExamAttempt.Add(attempt);
            await _context.SaveChangesAsync();
        }
    }
}