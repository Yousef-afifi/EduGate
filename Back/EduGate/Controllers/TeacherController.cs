using EduGate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduGate.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Courses()
        {
            return View();
        }
        public IActionResult Exams()
        {
            return View();
        }
        public IActionResult Students()
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
