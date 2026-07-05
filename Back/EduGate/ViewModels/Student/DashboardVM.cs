namespace EduGate.ViewModels.Student
{
    public class DashboardVM
    {
        public int EnrolledCoursesCount { get; set; }  
        public int AvgScore { get; set; }              
        public int CompletedLessonsCount { get; set; } 
        public int UpcomingExamsCount { get; set; }    

       
        public List<CoursesVM> EnrolledCourses { get; set; } = new List<CoursesVM>();
        public List<RecentGradesVM> RecentGrades { get; set; } = new List<RecentGradesVM>();
        public List<ScheduleVM> Schedule { get; set; } = new List<ScheduleVM>();
    }

    public class CoursesVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RecentGradesVM
    {
        public int Id { get; set; }
        public string AssessmentName { get; set; } 
        public string CourseName { get; set; }
        public int StudentGrade { get; set; }
        public int? TotalMark { get; set; }

    }


    public class ScheduleVM
    {
        public string Day { get; set; }        
        public string EventName { get; set; }  
        public string Time { get; set; }       
    }
}
