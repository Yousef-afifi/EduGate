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
        public async Task<CourseDetailsVM> GetCourseDeatailsAsync(int studentId, int courseId)
        {

            var studentAttempts = await _context.ExamAttempt
                .Where(a => a.Student_Id == studentId)
                .ToListAsync();


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

            return new TakeExamVM
            {
                ExamId = quiz.Id,
                ExamName = quiz.Name,
                Duration = quiz.Duration ?? 0,
                TotalMarks = quiz.Total_Marks ?? 0,
                PassingPercentage = quiz.PassingPercentage,
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
            return new StudentExamsVM
            {
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
                    TotalMarks = e.Total_Marks ?? 0,
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
                    TotalMarks = e.Total_Marks ?? 0,
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

            if (exam == null)
                return null;

            if (exam.Type != ExamType.Exam)
                return null;

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

            return new TakeExamVM
            {
                ExamId = exam.Id,
                ExamName = exam.Name,
                Duration = exam.Duration ?? 0,
                TotalMarks = exam.Total_Marks ?? 0,
                PassingPercentage = exam.PassingPercentage,
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

       
    }
}
