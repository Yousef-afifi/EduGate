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
            var result = await _service.Login(model);

            if(result.Success)
            {
                HttpContext.Session.SetInt32("UserId", result.Id);
                HttpContext.Session.SetString("Role", result.Role);

                if (result.Role == "Teacher")
                    return RedirectToAction("Dashboard", "Teacher");
                return RedirectToAction("Dashboard", "Student");
            }

            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
