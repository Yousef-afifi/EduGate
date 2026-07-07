using EduGate.Data;
using EduGate.Models;
using EduGate.Services.StuServices;
using EduGate.ViewModels.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
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
        public async Task<IActionResult> Dashboard()
        {
            int studentId = UserId.Value;

            var data = await _service.GetStudentOverview(studentId);

            return View(data);
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
        [HttpGet]
        public async Task<IActionResult> Submit_Assessment(int assessmentid)
        {
            var studentid = UserId.Value;
            var data = await _service.GetSubmitAssessment(assessmentid, studentid);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Submit_Assessment(SubmitAssessmentVM model)
        {
            if(ModelState.IsValid)
            {
                var data = await _service.GetSubmitAssessment(model.AssessmentId, model.StudentId);

                model.StudentName = data.StudentName;
                model.Initials = data.Initials;
                model.AssessmentTitle = data.AssessmentTitle;
                model.Instructions = data.Instructions;
                model.QuestionId = data.QuestionId;
                model.CourseId = data.CourseId;
                model.TotalMark = data.TotalMark;
                model.DueDate = data.DueDate;

                View(model);
            }

            await _service.SubmitAssessment(model);

            return RedirectToAction("Course_Details", "Student", new { id = model.CourseId });
        }
        public async Task<IActionResult> Schedule()
        {
            int StudentId = UserId.Value;

            var model = await _service.GetExamSchedule(StudentId);

            return View(model);
        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}
