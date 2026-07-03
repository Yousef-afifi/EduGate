using EduGate.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EduGate.Services.AuthServices
{
    public interface IAuthService
    {
        Task<LoginResultVM> Login(LoginVM model);
    }
}
