using EduGate.Data;
using EduGate.Models;
using EduGate.Services.StuServices;
using EduGate.Services.TeachService;
using EduGate.ViewModels.Teacher;
using EduGate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EduGate.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly ITeachService _service;
        public TeacherController(ITeachService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Dashboard()
        {
            var TeacherId = UserId.Value; 
            var data = await _service.GetDashboardData(TeacherId); 
            return View(data); 
        }
        public async Task<IActionResult> Courses()
        {
            var TeacherId = UserId.Value;
            var data = await _service.GetAllCoursesAsync(TeacherId);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Add_Course()
        {
            var TeacherId = UserId.Value;
            var data = await _service.GetAddCourse(TeacherId);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Add_Course(AddCourseVM model)
        {
            if (!ModelState.IsValid)
            {
                var data = await _service.GetAddCourse(model.TeacherId);

                model.TeacherName = data.TeacherName;
                model.Initials = data.Initials;

                View(model);
            }

            await _service.AddCourse(model);

            return RedirectToAction("Courses", "Teacher");
        }
        public async Task<IActionResult> Course_Details(int id)
        {
            var data = await _service.GetCourseDetailsAsync(id);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Add_Lesson(int id)
        {
            var data = await _service.GetAddLesson(id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Add_Lesson(AddLessonVM model)
        {
            if(!ModelState.IsValid)
            {
                var data = await _service.GetAddLesson(model.CourseId);

                model.TeacherName = data.TeacherName;
                model.CourseName = data.CourseName;
                model.Initials = data.Initials;

                return View(model);
            }

            await _service.AddLesson(model);

            return RedirectToAction("Course_Details", "Teacher", new {id = model.CourseId});
        }
        [HttpGet]
        public async Task<IActionResult> Add_Quiz(int id)
        {
            var data = await _service.GetAddQuiz(id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Add_Quiz(AddQuizVM model)
        {
            if(!ModelState.IsValid)
            {
                var data = await _service.GetAddQuiz(model.CourseId);

                model.CourseName = data.CourseName;
                model.TeacherName= data.TeacherName;
                model.Initials = data.Initials;

                return View(model);
            }

            await _service.AddQuiz(model);

            return RedirectToAction("Course_Details", "Teacher", new { id = model.CourseId });
        }
        [HttpGet]
        public async Task<IActionResult> Add_Assessment(int id)
        {
            var data = await _service.GetAddAssessment(id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Add_Assessment(AddAssessmentVM model)
        {
            if(!ModelState.IsValid)
            {
                var data = await _service.GetAddAssessment(model.CourseId);

                model.CourseName = data.CourseName;
                model.TeacherName = data.TeacherName;
                model.Initials = data.Initials;

                return View(model);
            }

            await _service.AddAssessment(model);

            return RedirectToAction("Course_Details", "Teacher", new { id = model.CourseId });
        }
        [HttpGet]
        public async Task<IActionResult> Upload_Material(int id)
        {
            var data = await _service.GetUploadMaterial(id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Upload_Material(UploadMaterialVM model)
        {
            if(!ModelState.IsValid)
            {
                var data = await _service.GetUploadMaterial(model.CourseId);

                model.CourseName = data.CourseName;
                model.TeacherName= data.TeacherName;
                model.Initials = data.Initials;

                return View(data);
            }

            await _service.UploadMaterial(model);

            return RedirectToAction("Course_Details", "Teacher", new { id = model.CourseId });
        }
        public async Task<IActionResult> Exams()
        {
            var teacherId = UserId.Value;
            var data = await _service.GetAllExams(teacherId);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Add_Exam()
        {
            var teacherId = UserId.Value;
            var data = await _service.GetAddExam(teacherId);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add_Exam(AddExamVM model)
        {
            if (!ModelState.IsValid)
            {
                var data = await _service.GetAddExam(UserId.Value);
                model.Courses = data.Courses;
                model.TeacherName = data.TeacherName;
                model.Initials = data.Initials;
                return View(model);
            }

            await _service.AddExam(model);
            return RedirectToAction(nameof(Exams));
        }

        [HttpGet]
        public async Task<IActionResult> Edit_Exam(int id)
        {
            var data = await _service.GetEditExam(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit_Exam(EditExamVM model)
        {
            if (!ModelState.IsValid) return View(model);

            await _service.EditExam(model);
            return RedirectToAction(nameof(Exams));
        }

        [HttpPost]
        public async Task<IActionResult> Delete_Exam(int id)
        {
            await _service.DeleteExam(id);
            return RedirectToAction(nameof(Exams));
        }
        public async Task<IActionResult> Students()
        {
            var TeacherId = UserId.Value;
            var data = await _service.GetAllStudentsAsync(TeacherId);
            return View(data);
        }
        public IActionResult Generate_Students()
        {
            return View();
        }
        public IActionResult Upgrade()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var TeacherId = UserId.Value;
            var model = await _service.GetSettings(TeacherId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(EduGate.ViewModels.Shared.SettingsVM model)
        {
            var TeacherId = UserId.Value;

            
            ModelState.Remove("CurrentPassword");
            ModelState.Remove("NewPassword");
            ModelState.Remove("ConfirmNewPassword");

            if (!ModelState.IsValid)
            {
                var currentData = await _service.GetSettings(TeacherId);
                model.TeacherName = currentData.TeacherName;
                model.Initials = currentData.Initials;
                return View("Settings", model); 
            }

            await _service.UpdateProfile(TeacherId, model);
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(EduGate.ViewModels.Shared.SettingsVM model)
        {
            var TeacherId = UserId.Value;

            
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("Email");

            if (!ModelState.IsValid)
            {
                var currentData = await _service.GetSettings(TeacherId);
                model.TeacherName = currentData.TeacherName;
                model.Initials = currentData.Initials;
                return View("Settings", model); 
            }

            var success = await _service.UpdatePassword(TeacherId, model);

            if (success)
            {
                return RedirectToAction("Settings");
            }
            else
            {
                ModelState.AddModelError("CurrentPassword", "Current password is incorrect");
                var currentData = await _service.GetSettings(TeacherId);
                model.TeacherName = currentData.TeacherName;
                model.Initials = currentData.Initials;
                return View("Settings", model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit_Quiz(int quizid)
        {
            var data = await _service.GetUpdateQuiz(quizid);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit_Quiz(EditQuizVM model)
        {
            if(!ModelState.IsValid)
            {
                var data = await _service.GetUpdateQuiz(model.QuizId);

                data.QuizTitle = model.QuizTitle;
                data.Date = model.Date;
                data.Time = model.Time;
                data.Duration = model.Duration;
                data.PassingScore = model.PassingScore;
                data.TotalMarks = model.TotalMarks;
                data.Questions = model.Questions;

                return View(data);
            }

            await _service.UpdateQuiz(model);

            return RedirectToAction("Course_Details", "Teacher", new { id = model.CourseId });
        }
        [HttpGet]
        public async Task<IActionResult> Edit_Lesson(int lessonid)
        {
            int teacherid = UserId.Value;
            var data = await _service.GetUpdateLesson(lessonid, teacherid);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit_Lesson(EditLessonVM model)
        {
            int teacherid = UserId.Value;
            if (!ModelState.IsValid)
            {
                var data = await _service.GetUpdateLesson(model.LessonId, teacherid);
                model.TeacherName = data.TeacherName;
                model.Initials = data.Initials;
                model.CourseId = data.CourseId;
                model.LessonTitle = data.LessonTitle;
                model.VideoURL = data.VideoURL;

                return View(model);
            }
            await _service.UpdateLesson(model);
            return RedirectToAction("Course_Details", "Teacher", new { id = model.CourseId });
        }
        [HttpGet]
        public async Task<IActionResult> Edit_Assesment(int assessmentid)
        {
            var data = await _service.GetUpdateAssessment(assessmentid);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit_Assessment(EditAssessmentVM model)
        {
            if(!ModelState.IsValid)
            {
                var data = await _service.GetUpdateAssessment(model.AssessmentId);

                model.CourseId = data.CourseId;
                model.AssessmentTitle = data.AssessmentTitle;
                model.TeacherName = data.TeacherName;
                model.Initials = data.Initials;
                model.Date = data.Date;
                model.Time = data.Time;
                model.TotalPoints = data.TotalPoints;
                model.PassingScore = data.PassingScore;
                model.Instructions = data.Instructions;

                return View(model);
            }
            await _service.UpdateAssessment(model);
            return RedirectToAction("Course_Details", "Teacher", new { id = model.CourseId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete_Lesson(int id, int courseId)
        {
            await _service.DeleteLesson(id);
            return RedirectToAction(nameof(Course_Details), new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete_Quiz(int id, int courseId)
        {
            await _service.DeleteExam(id);
            return RedirectToAction(nameof(Course_Details), new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete_Assessment(int id, int courseId)
        {
            await _service.DeleteExam(id);
            return RedirectToAction(nameof(Course_Details), new { id = courseId });
        }
    }
}
