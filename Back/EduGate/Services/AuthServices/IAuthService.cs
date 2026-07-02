using EduGate.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EduGate.Services.AuthServices
{
    public interface IAuthService
    {
        Task<string?> Login(LoginVM model);
    }
}
