using EduGate.Data;
using EduGate.Models;
using EduGate.Services.StuServices;
using EduGate.Services.TeachService;
using EduGate.ViewModels.Teacher;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduGate.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly ITeachService _service;
        public TeacherController(ITeachService service)
        {
            _service = service;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public async Task<IActionResult> Courses()
        {
            var TeacherId = UserId.Value;
            var data = await _service.GetAllCoursesAsync(TeacherId);
            return View(data);
        }
        public IActionResult Add_Course()
        {
            return View();
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
        public IActionResult Add_Assessment(int id)
        {
            return View();
        }
        public IActionResult Exams()
        {
            return View();
        }
        public IActionResult Add_Exam()
        {
            return View();
        }
        public IActionResult Edit_Exam(int id)
        {
            return View();
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
        public IActionResult Settings()
        {
            return View();
        }
    }
}
