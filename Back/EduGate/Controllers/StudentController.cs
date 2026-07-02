using EduGate.Data;
using EduGate.Models;
using EduGate.Services.StuServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduGate.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStuService _service;
        public StudentController(IStuService service)
        {
            _service = service;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public async Task<IActionResult> Courses()
        {
            int fakecurrentid = 3;
            var data = await _service.GetStudentCoursesAsync(fakecurrentid);
            return View(data);
        }
        public async Task<IActionResult> Course_Details(int id)
        {
            int fakecurrentid = 1;
            var data = await _service.GetCourseDeatailsAsync(fakecurrentid);
            return View(data);
        }
        public IActionResult Exams()
        {
            return View();
        }
        public IActionResult Take_Exam(int id)
        {
            return View();
        }
        public IActionResult Take_Quiz(int id)
        {
            return View();
        }
        public IActionResult Submit_Assessment(int id)
        {
            return View();
        }
        public IActionResult Schedule()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}
