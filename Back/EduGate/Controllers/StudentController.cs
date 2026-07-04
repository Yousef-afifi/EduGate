using EduGate.Data;
using EduGate.Models;
using EduGate.Services.StuServices;
using EduGate.ViewModels;
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
        public async Task<IActionResult> TakeExam(int id)
        {
            int studentId = UserId.Value;

            var model = await _service.StartExam(studentId, id);

            if (model == null)
                return RedirectToAction(nameof(Exams));

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitExam(SubmitExamVM model)
        {
            int studentId = UserId.Value;

            await _service.SubmitExam(studentId, model);

            return RedirectToAction(nameof(Exams));
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
