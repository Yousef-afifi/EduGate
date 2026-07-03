using EduGate.Data;
using EduGate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduGate.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly AppDbContext _context;
        public TeacherController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Courses()
        {
            return View();
        }
        public IActionResult Add_Course()
        {
            return View();
        }
        public IActionResult Course_Details(int id)
        {
            return View();
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
