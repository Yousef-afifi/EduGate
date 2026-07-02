using EduGate.Data;
using EduGate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduGate.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string?> Login(LoginVM model)
        {
            var stu = await _context.Account.FirstOrDefaultAsync(s => s.User_Name == model.Email);
            if(stu != null && stu.Password_Hash == model.Password)
            {
                return "Stu";
            }
            var user = await _context.Teacher.FirstOrDefaultAsync(t => t.Email == model.Email);
            if(user != null && user.Password_Hash == model.Password)
            {
                return "User";
            }
            return "Error";
        }
    }
}