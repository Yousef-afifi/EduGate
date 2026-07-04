using EduGate.Data;
using EduGate.Models;
using EduGate.Services.AuthServices;
using EduGate.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var vm = new RegisterVM();

            vm.Packages = await _service.GetPackagesAsync();

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if(!ModelState.IsValid)
            {
                model.Packages = await _service.GetPackagesAsync();
                return View(model);
            }
                
            var result = await _service.Register(model);

            if(!result.Success)
            {
                model.Packages = await _service.GetPackagesAsync();
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            return RedirectToAction("Login");
        }
    }
}
