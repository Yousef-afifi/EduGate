using EduGate.Data;
using EduGate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduGate.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;
        public StudentController(AppDbContext context)
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
        public IActionResult Course_Details(int id)
        {
            return View();
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
