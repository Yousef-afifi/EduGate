using EduGate.Data;
using EduGate.Enums;
using EduGate.Models;
using EduGate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<RegisterResultVM>Register(RegisterVM model) 
        {
            var exists = await _context.Teacher.AllAsync(x => x.Email == model.Email);

            if(exists) 
            {
                return new RegisterResultVM
                {
                    Success = false,
                    Message = "Email already exist"
                };
            }

            var teacher = new Teacher
            {
                First_Name = model.First_Name,
                Last_Name = model.Last_Name,
                Gender = model.Gender,
                Email = model.Email,
                Password_Hash = model.Password,
                Phone = model.Phone,
                CreatedAt = DateTime.Now
            };

            _context.Teacher.Add(teacher);
            await _context.SaveChangesAsync();

            var subscription = new Subscription
            {
                Start_Date = DateOnly.FromDateTime(DateTime.Now),
                End_Date = DateOnly.FromDateTime(DateTime.Now.AddMonths(3)),
                Status = SubscriptionStatus.Active,
                Package_Id = model.Package_Id,
                Teacher_Id = teacher.Id
            };
            
            _context.Subscription.Add(subscription);
            await _context.SaveChangesAsync();

            return new RegisterResultVM { Success = true };
        }
        public async Task<IEnumerable<SelectListItem>> GetPackagesAsync()
        {
            return await _context.Package
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .ToListAsync();
        }
    }
}