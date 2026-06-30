using EduGate.Data;
using EduGate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduGate.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
