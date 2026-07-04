using EduGate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduGate.Services.AuthServices
{
    public interface IAuthService
    {
        Task<LoginResultVM> Login(LoginVM model);
        Task<RegisterResultVM> Register(RegisterVM model);
        Task<IEnumerable<SelectListItem>> GetPackagesAsync();
    }
}
