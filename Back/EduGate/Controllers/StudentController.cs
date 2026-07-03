using EduGate.Data;
using EduGate.Models;
using EduGate.Services.StuServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduGate.Controllers
{
    public class StudentController : BaseController
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
            var StudentId = UserId.Value;
            var data = await _service.GetStudentCoursesAsync(StudentId);
            return View(data);
        }
        public async Task<IActionResult> Course_Details(int id)
        {
            int StudentId = UserId.Value;
            var data = await _service.GetCourseDeatailsAsync(StudentId);
            return View(data);
        }
        public async Task<IActionResult> Exams()
        {
            int StudentId = UserId.Value; 

            var model = await _service.GetStudentExams(StudentId);

            return View(model);
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
