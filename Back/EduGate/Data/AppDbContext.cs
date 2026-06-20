using Microsoft.EntityFrameworkCore;
using EduGate.Models;
using System.Diagnostics;
namespace EduGate.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Account> Account { get; set; }
        public DbSet<Choice> Choice { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<Exam> Exam { get; set; }
        public DbSet<ExamAttempt> ExamAttempt { get; set; }
        public DbSet<Lesson> Lesson { get; set; }
        public DbSet<LessonMaterial> Material { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentAnswer> StudentAnswer { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
     
    }
}
