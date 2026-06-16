using EduGate.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string User_Name { get; set; }
        public string Password_Hash { get; set; }
        public AccountStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("student")]
        public int Student_Id { get; set; }
        public Student student { get; set; }
        [ForeignKey("teacher")]
        public int Teacher_Id { get; set; }
        public Teacher teacher { get; set; }
    }
}
