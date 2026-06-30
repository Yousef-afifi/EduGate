using EduGate.Enums;
using EduGate.Models;

namespace EduGate.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Teacher.Any())
                {
                    context.Teacher.AddRange(new List<Teacher>()
                    {
                        new Teacher()
                        {
                            First_Name = "Mohamed",
                            Last_Name = "Tawfik",
                            Gender =Enums.Gender.Male,
                            Email = "mohamedtawfik34@gmail.com",
                            Password_Hash = "Hashed_pasword",
                            Phone = "01299783142",
                            CreatedAt = DateTime.Now,
                        },
                        new Teacher()
                        {
                            First_Name = "Abdallah",
                            Last_Name = "Ismail",
                            Gender =Enums.Gender.Male,
                            Email = "A_Ismail400@gmail.com",
                            Password_Hash = "Hashed_pasword",
                            Phone = "01542217836",
                            CreatedAt = DateTime.Now,
                        },
                        new Teacher()
                        {
                            First_Name = "Nawal",
                            Last_Name = "Mohamed",
                            Gender =Enums.Gender.Female,
                            Email = "Nawal778@gmail.com",
                            Password_Hash = "Hashed_pasword",
                            Phone = "01123479120",
                            CreatedAt = DateTime.Now,
                        },
                        new Teacher()
                        {
                            First_Name = "Ibrahim",
                            Last_Name = "Gomaa",
                            Gender =Enums.Gender.Male,
                            Email = "EB_Gomaaaa12@gmail.com",
                            Password_Hash = "Hashed_pasword",
                            Phone = "010089125631",
                            CreatedAt = DateTime.Now,
                        }
                    });
                    context.SaveChanges();
                }
                
                if (!context.Student.Any())
                {
                    context.Student.AddRange(new List<Student>()
                    {
                        new Student()
                        {
                            First_Name = "Ahmed",
                            Last_Name = "Ali",
                            Gender =Enums.Gender.Male,
                            Phone = "01007789553"
                        },

                        new Student()
                        {
                            First_Name = "Salma",
                            Last_Name = "Ahmed",
                            Gender =Enums.Gender.Female,
                            Phone = "01127689572"
                        },
                        new Student()
                        {
                            First_Name = "Amir",
                            Last_Name = "Mohamed",
                            Gender =Enums.Gender.Male,
                            Phone = "01553379152"
                        },
                        new Student()
                        {
                            First_Name = "Mohamed",
                            Last_Name = "Ibrahim",
                            Gender =Enums.Gender.Male,
                            Phone = "01059672841"
                        },
                        new Student()
                        {
                            First_Name = "Abdallah",
                            Last_Name = "Mohamed",
                            Gender =Enums.Gender.Male,
                            Phone = "01005474338"
                        },
                        new Student()
                        {
                            First_Name = "Menna",
                            Last_Name = "Mahmoud",
                            Gender =Enums.Gender.Female,
                            Phone = "01259966444"
                        },
                        new Student()
                        {
                            First_Name = "Mohamed",
                            Last_Name = "Fathi",
                            Gender =Enums.Gender.Male,
                            Phone = "01124196175"
                        },
                        new Student()
                        {
                            First_Name = "Yara",
                            Last_Name = "Hamdy",
                            Gender =Enums.Gender.Female,
                            Phone = "01002541339"
                        },
                    });
                    context.SaveChanges();
                }

                
                if (!context.Account.Any())
                {
                    context.Account.AddRange(new List<Account>()
                    {
                        new Account
                        {
                            User_Name = "ahmed_ali",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active, 
                            CreatedAt = DateTime.Now,
                            Student_Id = 1, 
                            Teacher_Id = 1  
                        },
                        new Account
                        {
                            User_Name = "salma_ahmed",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 2,
                            Teacher_Id = 1
                        },
                        new Account
                        {
                            User_Name = "amir_mohamed",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 3, 
                            Teacher_Id = 2
                        },
                        new Account
                        {
                            User_Name = "mohamed_ibrahim",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 4,
                            Teacher_Id = 2
                        },
                        new Account
                        {
                            User_Name = "Abdallah_mohamed",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 5,
                            Teacher_Id = 3
                        },
                        new Account
                        {
                            User_Name = "menna_mahmoud",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 6,
                            Teacher_Id = 4
                        },
                        new Account
                        {
                            User_Name = "mohamed_fathi",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 7,
                            Teacher_Id = 4
                        },
                        new Account
                        {
                            User_Name = "yara_hamdy",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 8,
                            Teacher_Id = 3
                        },

                    });
                    context.SaveChanges();
                }
                if (!context.Package.Any())
                {
                    context.Package.AddRange(new List<Package>()
                    {
                        new Package()
                        {
                            Name = "Starter",
                            Description = "Perfect for individual teachers launching their first online class." ,
                            Price = 100.0,
                            Max_Students = 200
                        },
                        new Package()
                        {
                            Name = "Growth",
                            Description = "Best for teachers with multiple groups and recurring lecture drops." ,
                            Price = 270.0,
                            Max_Students = 600
                        },
                        new Package()
                        {
                            Name = "Scale",
                            Description = "For established teachers and academies serving large student communities." ,
                            Price = 450.0,
                            Max_Students = 1000
                        },
                    });
                    context.SaveChanges();
                }
                if (!context.Course.Any())
                {
                    context.Course.AddRange(new List<Course>()
                    {
                        new Course()
                        {
                            Name = "C# Programming Course",
                            Description = "Learn programming basics and clean code principles.",
                            CreatedAt = DateTime.Now,
                            Teacher_Id = 1
                        },
                        new Course()
                        {
                            Name = "ASP.NET Core Web Development",
                            Description = "Build full-stack web applications from scratch.",
                            CreatedAt = DateTime.Now,
                            Teacher_Id = 2
                        },
                        new Course
                        {
                            Name = "SQL Server Database Design",
                            Description = "Learn relational database design, normalization, complex queries, and indexing.",
                            CreatedAt = DateTime.Now,
                            Teacher_Id = 3
                        },
                        new Course
                        {
                            Name = "Frontend Fundamentals with HTML, CSS & JS",
                            Description = "Build responsive and interactive user interfaces from scratch.",
                            CreatedAt = DateTime.Now,
                            Teacher_Id = 4
                        },

                    });
                    context.SaveChanges();
                }

                if (!context.Subscription.Any())
                {
                    context.Subscription.AddRange(new List<Subscription>() 
                    {
                         new Subscription() 
                         {
                             Start_Date = new DateOnly(2027, 1, 1), 
                             End_Date = new DateOnly(2027, 5, 1),
                             Status = SubscriptionStatus.Active,
                             Package_Id = 1,
                             Teacher_Id = 1
                         },
                         new Subscription() 
                         {
                             Start_Date = new DateOnly(2026, 3, 15), 
                             End_Date = new DateOnly(2026, 6, 15),
                             Status = SubscriptionStatus.Expired,
                             Package_Id = 2,
                             Teacher_Id = 2
                         },
                         new Subscription() 
                         {
                             Start_Date = new DateOnly(2026, 7, 10), 
                             End_Date = new DateOnly(2026, 12, 10),
                             Status = SubscriptionStatus.Active,
                             Package_Id = 3,
                             Teacher_Id = 3
                         },
                         new Subscription() 
                         {
                             Start_Date = new DateOnly(2026, 9, 1), 
                             End_Date = new DateOnly(2027, 3, 1),
                             Status = SubscriptionStatus.Active,
                             Package_Id = 2,
                             Teacher_Id = 4 

                         },

                    }); 
                    context.SaveChanges();
                }
                if (!context.Enrollment.Any())
                {
                    context.Enrollment.AddRange(new List<Enrollment>()
                    {
                         new Enrollment
                         {
                             Student_Id = 1,
                             Course_Id = 1
                         },
                         new Enrollment
                         {
                             Student_Id = 2,
                             Course_Id = 1
                         },
                         new Enrollment
                         {
                            Student_Id = 3,
                             Course_Id = 2
                         },
                         new Enrollment
                         {
                             Student_Id = 4,
                             Course_Id = 2
                         },
                         new Enrollment
                         {
                             Student_Id = 5,
                             Course_Id = 3
                         },
                         new Enrollment
                         {
                             Student_Id = 6,
                             Course_Id = 4
                         },
                         new Enrollment
                         {
                             Student_Id = 7,
                             Course_Id = 4
                         },
                         new Enrollment
                         {
                             Student_Id = 8,
                             Course_Id = 3
                         },

                    });
                    context.SaveChanges();
                }

                if (!context.Lesson.Any())
                {
                    context.Lesson.AddRange(new List<Lesson>()
                    {
                         new Lesson
                         {
                              Name = "Introduction to C# & .NET",
                              Video_Url = "https://edugate.com/videos/csharp-intro",
                              CreatedAt = DateTime.Now,
                              Course_Id = 1
                         },
                         new Lesson
                         {
                               Name = "Variables and Data Types",
                               Video_Url = "https://edugate.com/videos/csharp-variables",
                               CreatedAt = DateTime.Now,
                               Course_Id = 1
                         },
                         new Lesson
                         {
                               Name = "Understanding MVC Architecture",
                               Video_Url = "https://edugate.com/videos/mvc-basics",
                               CreatedAt = DateTime.Now,
                               Course_Id = 2
                        },
                        new Lesson
                        {
                               Name = "Dependency Injection in .NET",
                               Video_Url = "https://edugate.com/videos/dotnet-di",
                               CreatedAt = DateTime.Now,
                               Course_Id = 2
                        },
                        new Lesson
                        {
                               Name = "Introduction to Relational Databases",
                               Video_Url = "https://edugate.com/videos/csharp-variables",
                               CreatedAt = DateTime.Now,
                               Course_Id = 3
                        },
                        new Lesson
                        {
                               Name = "Mastering Joins and Subqueries",
                               Video_Url = "https://edugate.com/videos/sql-joins",
                               CreatedAt = DateTime.Now,
                               Course_Id = 3
                        },
                        new Lesson
                        {
                               Name = "HTML5 Semantic Elements",
                               Video_Url = "https://edugate.com/videos/html-basics",
                               CreatedAt = DateTime.Now,
                               Course_Id = 4
                        },
                        new Lesson
                        {
                               Name = "CSS Flexbox and Grid Layouts",
                               Video_Url = "https://edugate.com/videos/css-layouts",
                               CreatedAt = DateTime.Now,
                               Course_Id = 4
                        },
                    });
                    context.SaveChanges();
                }

                if (!context.Material.Any())
                {
                    context.Material.AddRange(new List<LessonMaterial>()
                    {
                        new LessonMaterial
                        {
                             File_Path = "https://edugate.com/materials/csharp-intro.pdf",
                             File_Type = "PDF",
                             Lesson_Id = 1
                        },
                        new LessonMaterial
                        {
                             File_Path = "https://edugate.com/materials/mvc-diagrams.zip",
                             File_Type = "ZIP",
                             Lesson_Id = 3
                        },
                        new LessonMaterial
                        {
                            File_Path = "https://edugate.com/materials/sql-cheatsheet.pdf",
                            File_Type = "PDF",
                            Lesson_Id = 5
                        },
                        new LessonMaterial
                        {
                            File_Path = "https://edugate.com/materials/html5-guide.pdf",
                            File_Type = "PDF",
                            Lesson_Id = 7
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Exam.Any())
                {
                    context.Exam.AddRange(new List<Exam>()
                    {
                        new Exam
                        {
                            Name = "C# Basics Quiz",
                            Type = ExamType.Quiz,
                            Duration = 30,
                            Total_Marks = 50,
                            CreatedAt = DateTime.Now,
                            Course_Id = 1
                        },
                        new Exam
                        {
                             Name = "ASP.NET Core",
                             Type = ExamType.Assignment,
                             Duration = 120,
                             Total_Marks = 10,
                             CreatedAt = DateTime.Now,
                             Course_Id = 2
                        },
                        new Exam
                        {
                             Name = "Sql Basics",
                             Type = ExamType.Exam,
                             Duration = 120,
                             Total_Marks = 10,
                             CreatedAt = DateTime.Now,
                             Course_Id = 3
                        },

                    });

                    context.SaveChanges();
                }

                if (!context.Question.Any())
                {
                    context.Question.AddRange(new List<Question>()
                    {
                         new Question
                         {
                              Text = "What is the entry point of a C# console application?",
                              Mark = 25,
                              Exam_Id = 1
                         },
                         new Question
                         {
                             Text = "Which datatype is used to store a single character in C#?",
                             Mark = 25,
                             Exam_Id = 1
                         },
                         new Question
                         {
                             Text = "Which lifecycle method registers a service as a single instance across the app?",
                             Mark = 10,
                             Exam_Id = 2
                         },
                         new Question
                         {
                             Text = "Which SQL command to get all data from the table.",
                             Mark = 10,
                             Exam_Id = 3
                         },

                    });
                    context.SaveChanges();
                }

                if (!context.Choice.Any())
                {
                    context.Choice.AddRange(new List<Choice>()
                    {
                         new Choice { Text = "Main method", IsCorrect = true, Question_Id = 1 },
                         new Choice { Text = "Start method", IsCorrect = false, Question_Id = 1 },
                         new Choice { Text = "Page_Load", IsCorrect = false, Question_Id = 1 },


                         new Choice { Text = "string", IsCorrect = false, Question_Id = 2 },
                         new Choice { Text = "char", IsCorrect = true, Question_Id = 2 },
                         new Choice { Text = "int", IsCorrect = false, Question_Id = 2 },


                         new Choice { Text = "AddTransient", IsCorrect = false, Question_Id = 3 },
                         new Choice { Text = "AddScoped", IsCorrect = false, Question_Id = 3 },
                         new Choice { Text = "AddSingleton", IsCorrect = true, Question_Id = 3 },

                         new Choice { Text = "SELECT * FROM TableName", IsCorrect = true, Question_Id = 4 },
                         new Choice { Text = "GET * FROM TableName", IsCorrect = false, Question_Id = 4 },
                         new Choice { Text = "EXTRACT ALL FROM TableName", IsCorrect = false, Question_Id = 4 },
                         new Choice { Text = "FETCH ALL FROM TableName", IsCorrect = false, Question_Id = 4 }
                    });
                    context.SaveChanges();
                }

                if (!context.ExamAttempt.Any())
                {
                    context.ExamAttempt.AddRange(new List<ExamAttempt>()
                    {
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 50,                   
                             StartedAt = DateTime.Now.AddHours(-2),
                             SubmittedAt = DateTime.Now.AddHours(-1),
                             Student_Id = 1,               
                             Exam_Id = 1
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 10,
                             StartedAt = DateTime.Now.AddHours(-45),
                             SubmittedAt = DateTime.Now.AddHours(-15),
                             Student_Id = 2,
                             Exam_Id = 3
                        }
                    });
                    context.SaveChanges();

                }
                if (!context.StudentAnswer.Any())
                {
                    context.StudentAnswer.AddRange(new List<StudentAnswer>
                    {
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.5),
                           Student_Id = 1,
                           Question_Id = 1,
                           Choice_Id = 1
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.4),
                           Student_Id = 1,
                           Question_Id = 2,
                           Choice_Id = 5
                       },
                       new StudentAnswer
                       { 
                           AnswerdAt = DateTime.Now.AddMinutes(-30),
                           Student_Id = 2,
                           Question_Id = 4,
                           Choice_Id = 10
                       }
                    });
                    context.SaveChanges();
                }

            }
        }
    }
}


