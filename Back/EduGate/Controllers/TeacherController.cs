using EduGate.Data;
using EduGate.Models;
using EduGate.Services.StuServices;
using EduGate.Services.TeachService;
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
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }
        public IActionResult Add_Lesson(int id)
        {
            return View();
        }
        public IActionResult Add_Quiz(int id)
        {
            return View();
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
        public IActionResult Students()
        {
            return View();
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
