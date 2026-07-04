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
            int studentId = UserId.Value;
            var data = await _service.GetCourseDeatailsAsync(studentId, id);
            if (data == null)
            {
                return NotFound(); 
            }
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
        public async Task<IActionResult> Take_Quiz(int id)
        {
            int studentId = UserId.Value;

            var model = await _service.TakeQuiz(studentId, id);

            if (model == null)
                return RedirectToAction(nameof(Course_Details));

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(SubmitExamVM model)
        {
            int studentId = UserId.Value;

            var courseid = await _service.SubmitQuiz(studentId, model);

            return RedirectToAction(nameof(Course_Details), new { id = courseid });
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
