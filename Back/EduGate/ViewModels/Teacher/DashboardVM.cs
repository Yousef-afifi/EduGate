namespace EduGate.ViewModels.Teacher
{
  

    public class DashboardVM
    {
        public int TotalStudents { get; set; }
        public int ActiveCourses { get; set; }
        public int UpcomingExams { get; set; }
        public string TeacherName { get; set; }
        public string Initials { get; set; }

        public List<RecentCourseVM> RecentCourses { get; set; }
    }
}
