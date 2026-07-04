namespace EduGate.ViewModels.Auth
{
    public class LoginVM
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginResultVM
    {
        public bool Success { get; set; }
        public int Id { get; set; }
        public string Role { get; set; }
    }
}
