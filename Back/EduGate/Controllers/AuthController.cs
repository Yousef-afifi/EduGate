using EduGate.Models;
using EduGate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduGate.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (model.Email.StartsWith("teacher"))
            {
                return RedirectToAction("Dashboard", "Teacher");
            }

            if (model.Email.StartsWith("student"))
            {
                return RedirectToAction("Dashboard", "Student");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
