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
        public async Task<LoginResultVM> Login(LoginVM model)
        {
            var stu = await _context.Account.FirstOrDefaultAsync(s => s.User_Name == model.Email);
            if(stu != null && stu.Password_Hash == model.Password)
            {
                return new LoginResultVM
                {
                    Success = true,
                    Id = stu.Student_Id,
                    Role = "Student"
                };
            }
            var user = await _context.Teacher.FirstOrDefaultAsync(t => t.Email == model.Email);
            if(user != null && user.Password_Hash == model.Password)
            {
                return new LoginResultVM
                {
                    Success = true,
                    Id = user.Id,
                    Role = "Teacher"
                };
            }
            return new LoginResultVM
            {
                Success = false
            };
        }
    }
}