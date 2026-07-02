using EduGate.Data;
using EduGate.Models;
using EduGate.Services.AuthServices;
using EduGate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduGate.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var User_Type = await _service.Login(model);
            if(User_Type == "User")
            {
                return RedirectToAction("Dashboard", "Teacher");
            }

            if(User_Type == "Stu")
            {
                return RedirectToAction("Dashboard", "Student");
            }
            
            if(User_Type == "Error")
            {
                ModelState.AddModelError("", "Invalid username or password");
            }
            
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
