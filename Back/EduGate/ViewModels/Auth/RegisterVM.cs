using EduGate.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EduGate.ViewModels.Auth
{
    public class RegisterVM
    {
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string Last_Name { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public int Package_Id { get; set; }
        [Range(typeof(bool), "true", "true",
        ErrorMessage = "You must accept Terms")]
        public bool AcceptTerms { get; set; }
        public IEnumerable<SelectListItem>? Packages { get; set; }
    }
    public class RegisterResultVM
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
