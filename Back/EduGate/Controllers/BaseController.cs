using Microsoft.AspNetCore.Mvc;

namespace EduGate.Controllers
{
    public class BaseController : Controller
    {
        protected int? UserId =>
            HttpContext.Session.GetInt32("UserId");

        protected string? Role =>
            HttpContext.Session.GetString("Role");
    }
}
