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
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Account.Any())
                {
                    context.Account.AddRange(new List<Account>()
                    {
                        new Account
                        {
                            User_Name = "ahmed_ali@stu.com",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active, 
                            CreatedAt = DateTime.Now,
                            Student_Id = 1, 
                            Teacher_Id = 1  
                        },
                        new Account
                        {
                            User_Name = "salma_ahmed@stu.com",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 2,
                            Teacher_Id = 1
                        },
                        new Account
                        {
                            User_Name = "amir_mohamed@stu.com",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 3, 
                            Teacher_Id = 2
                        },
                        new Account
                        {
                            User_Name = "mohamed_ibrahim@stu.com",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 4,
                            Teacher_Id = 2
                        },
                        new Account
                        {
                            User_Name = "Abdallah_mohamed@stu.com",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 5,
                            Teacher_Id = 3
                        },
                        new Account
                        {
                            User_Name = "menna_mahmoud@stu.com",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 6,
                            Teacher_Id = 3
                        },
                        new Account
                        {
                            User_Name = "mohamed_fathi@stu.com",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 7,
                            Teacher_Id = 4
                        },
                        new Account
                        {
                            User_Name = "yara_hamdy@stu.com",
                            Password_Hash = "student_password_hash",
                            Status = AccountStatus.Active,
                            CreatedAt = DateTime.Now,
                            Student_Id = 8,
                            Teacher_Id = 4
                        }
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
                        }
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
                            Teacher_Id = 1
                        },
                        new Course
                        {
                            Name = "SQL Server Database Design",
                            Description = "Learn relational database design, normalization, complex queries, and indexing.",
                            CreatedAt = DateTime.Now,
                            Teacher_Id = 1
                        },
                        new Course
                        {
                            Name = "Frontend Fundamentals with HTML, CSS & JS",
                            Description = "Build responsive and interactive user interfaces from scratch.",
                            CreatedAt = DateTime.Now,
                            Teacher_Id = 1
                        },
                        new Course
                        {
                            Name = "Digital Signal Proccessing",
                            Description = "transforms real-world analog phenomena—like sound, light, or temperature—into discrete numerical sequences.",
                            CreatedAt= DateTime.Now,
                            Teacher_Id = 2
                        },
                        new Course
                        {
                            Name = "Image Proccessing",
                            Description = "the systematic manipulation of visual data—like photos, video frames, or medical scans—using computer algorithms.",
                            CreatedAt= DateTime.Now,
                            Teacher_Id = 2
                        },
                        new Course
                        {
                            Name = "Calculs II",
                            Description = "the study of integration, infinite series, and their practical applications.",
                            CreatedAt= DateTime.Now,
                            Teacher_Id = 3
                        },
                        new Course
                        {
                            Name = "Physics",
                            Description = "the natural science that studies matter, energy, and the fundamental forces that govern the universe.",
                            CreatedAt= DateTime.Now,
                            Teacher_Id = 3
                        },
                        new Course
                        {
                            Name = "C++ Programming Course",
                            Description = "Learn programming basics and clean code principles.",
                            CreatedAt= DateTime.Now,
                            Teacher_Id = 4
                        },
                        new Course
                        {
                            Name = "Data Structure",
                            Description = "a specialized format for organizing, processing, storing, and retrieving data efficiently in a computer's memory.",
                            CreatedAt= DateTime.Now,
                            Teacher_Id = 4
                        }
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
                             Package_Id = 1,
                             Teacher_Id = 2
                         },
                         new Subscription() 
                         {
                             Start_Date = new DateOnly(2026, 7, 10), 
                             End_Date = new DateOnly(2026, 12, 10),
                             Status = SubscriptionStatus.Active,
                             Package_Id = 2,
                             Teacher_Id = 3
                         },
                         new Subscription() 
                         {
                             Start_Date = new DateOnly(2026, 9, 1), 
                             End_Date = new DateOnly(2027, 3, 1),
                             Status = SubscriptionStatus.Active,
                             Package_Id = 3,
                             Teacher_Id = 4 

                         }
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
                             Student_Id = 1,
                             Course_Id = 2
                         },
                         new Enrollment
                         {
                             Student_Id = 2,
                             Course_Id = 2
                         },
                         new Enrollment
                         {
                             Student_Id = 1,
                             Course_Id = 3
                         },
                         new Enrollment
                         {
                             Student_Id = 2,
                             Course_Id = 3
                         },
                         new Enrollment
                         {
                             Student_Id = 1,
                             Course_Id = 4
                         },
                         new Enrollment
                         {
                             Student_Id = 2,
                             Course_Id = 4
                         },
                         new Enrollment
                         {
                            Student_Id = 3,
                             Course_Id = 5
                         },
                         new Enrollment
                         {
                             Student_Id = 4,
                             Course_Id = 5
                         },
                         new Enrollment
                         {
                            Student_Id = 3,
                             Course_Id = 6
                         },
                         new Enrollment
                         {
                             Student_Id = 4,
                             Course_Id = 6
                         },
                         new Enrollment
                         {
                             Student_Id = 5,
                             Course_Id = 7
                         },
                         new Enrollment
                         {
                             Student_Id = 6,
                             Course_Id = 7
                         },
                         new Enrollment
                         {
                             Student_Id = 5,
                             Course_Id = 8
                         },
                         new Enrollment
                         {
                             Student_Id = 6,
                             Course_Id = 8
                         },
                         new Enrollment
                         {
                             Student_Id = 7,
                             Course_Id = 9
                         },
                         new Enrollment
                         {
                             Student_Id = 8,
                             Course_Id = 9
                         },
                         new Enrollment
                         {
                             Student_Id = 7,
                             Course_Id = 10
                         },
                         new Enrollment
                         {
                             Student_Id = 8,
                             Course_Id = 10
                         }
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
                        new Lesson
                        {
                               Name = "Intro to Digital Signal Processing",
                               Video_Url = "https://edugate.com/videos/intro-to-dsp",
                               CreatedAt = DateTime.Now,
                               Course_Id = 5
                        },
                        new Lesson
                        {
                               Name = "Intro to Image Processing",
                               Video_Url = "https://edugate.com/videos/intro-to-ip",
                               CreatedAt = DateTime.Now,
                               Course_Id = 6
                        },
                        new Lesson
                        {
                               Name = "Definition of Limits",
                               Video_Url = "https://edugate.com/videos/limits",
                               CreatedAt = DateTime.Now,
                               Course_Id = 7
                        },
                        new Lesson
                        {
                               Name = "Waves (Analogue and Digital Signals)",
                               Video_Url = "https://edugate.com/videos/waves",
                               CreatedAt = DateTime.Now,
                               Course_Id = 8
                        },
                        new Lesson
                        {
                               Name = "Variables and Data Types",
                               Video_Url = "https://edugate.com/videos/cpp-variables",
                               CreatedAt = DateTime.Now,
                               Course_Id = 9
                        },
                        new Lesson
                        {
                               Name = "Lists & Arrays",
                               Video_Url = "https://edugate.com/videos/cpp-list",
                               CreatedAt = DateTime.Now,
                               Course_Id = 10
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Material.Any())
                {
                    context.Material.AddRange(new List<LessonMaterial>()
                    {
                        new LessonMaterial
                        {
                             Title = "Intro to C#.pdf",
                             File_Path = "https://edugate.com/materials/csharp-intro.pdf",
                             File_Type = "PDF",
                             File_Size = 1.7,
                             Lesson_Id = 1
                        },
                        new LessonMaterial
                        {
                             Title = "MVC Diagrams.zip",
                             File_Path = "https://edugate.com/materials/mvc-diagrams.zip",
                             File_Type = "ZIP",
                             File_Size = 2.8,
                             Lesson_Id = 3
                        },
                        new LessonMaterial
                        {
                            Title = "SQL Sheet.PDF",
                            File_Path = "https://edugate.com/materials/sql-cheatsheet.pdf",
                            File_Type = "PDF",
                            File_Size = 1.2,
                            Lesson_Id = 5
                        },
                        new LessonMaterial
                        {
                            Title = "HTML Guide.pdf",
                            File_Path = "https://edugate.com/materials/html5-guide.pdf",
                            File_Type = "PDF",
                            File_Size = 1.9,
                            Lesson_Id = 7
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Exam.Any())
                {
                    context.Exam.AddRange(new List<Exam>()
                    {
                        // Completed
                        new Exam
                        {
                            Name = "Variables Quiz",
                            Type = ExamType.Quiz,
                            Duration = 30,
                            Total_Marks = 50,
                            PassingPercentage = 60,
                            StartDate = DateTime.Now.AddDays(-7),
                            CreatedAt = DateTime.Now,
                            Course_Id = 1
                        },
                        new Exam
                        {
                            Name = "C# Basics Assignment",
                            Type = ExamType.Assignment,
                            Duration = 30,
                            Total_Marks = 50,
                            PassingPercentage = 60,
                            StartDate = DateTime.Now.AddDays(-7),
                            CreatedAt = DateTime.Now,
                            Course_Id = 1
                        },
                        new Exam
                        {
                            Name = "Variables Exam",
                            Type = ExamType.Exam,
                            Duration = 30,
                            Total_Marks = 50,
                            PassingPercentage = 60,
                            StartDate = DateTime.Now.AddDays(-7),
                            CreatedAt = DateTime.Now,
                            Course_Id = 1
                        },
                        new Exam
                        {
                            Name = "API Quiz",
                            Type = ExamType.Quiz,
                            Duration = 120,
                            Total_Marks = 10,
                            PassingPercentage = 50,
                            StartDate = DateTime.Now.AddDays(-3),
                            CreatedAt = DateTime.Now,
                            Course_Id = 2
                        },
                        new Exam
                        {
                            Name = "API Assignment",
                            Type = ExamType.Assignment,
                            Duration = 120,
                            Total_Marks = 10,
                            PassingPercentage = 50,
                            StartDate = DateTime.Now.AddDays(-3),
                            CreatedAt = DateTime.Now,
                            Course_Id = 2
                        },
                        new Exam
                        {
                            Name = "API Exam",
                            Type = ExamType.Exam,
                            Duration = 120,
                            Total_Marks = 10,
                            PassingPercentage = 50,
                            StartDate = DateTime.Now.AddDays(-3),
                            CreatedAt = DateTime.Now,
                            Course_Id = 2
                        },
                        new Exam
                        {
                            Name = "SQL Basics Exam",
                            Type = ExamType.Exam,
                            Duration = 120,
                            Total_Marks = 10,
                            PassingPercentage = 70,
                            StartDate = DateTime.Now.AddDays(-5),
                            CreatedAt = DateTime.Now,
                            Course_Id = 3
                        },
                        // Upcoming
                        new Exam
                        {
                            Name = "OOP Quiz",
                            Type = ExamType.Quiz,
                            Duration = 30,
                            Total_Marks = 50,
                            PassingPercentage = 60,
                            StartDate = DateTime.Now.AddDays(7),
                            CreatedAt = DateTime.Now,
                            Course_Id = 1
                        },
                        new Exam
                        {
                            Name = "OOP Assignment",
                            Type = ExamType.Assignment,
                            Duration = 30,
                            Total_Marks = 50,
                            PassingPercentage = 60,
                            StartDate = DateTime.Now.AddDays(7),
                            CreatedAt = DateTime.Now,
                            Course_Id = 1
                        },
                        new Exam
                        {
                            Name = "OOP Exam",
                            Type = ExamType.Exam,
                            Duration = 30,
                            Total_Marks = 50,
                            PassingPercentage = 60,
                            StartDate = DateTime.Now.AddDays(7),
                            CreatedAt = DateTime.Now,
                            Course_Id = 1
                        },
                        new Exam
                        {
                            Name = "MVC Quiz",
                            Type = ExamType.Quiz,
                            Duration = 120,
                            Total_Marks = 10,
                            PassingPercentage = 50,
                            StartDate = DateTime.Now.AddDays(3),
                            CreatedAt = DateTime.Now,
                            Course_Id = 2
                        },
                        new Exam
                        {
                            Name = "MVC Assignment",
                            Type = ExamType.Assignment,
                            Duration = 120,
                            Total_Marks = 10,
                            PassingPercentage = 50,
                            StartDate = DateTime.Now.AddDays(3),
                            CreatedAt = DateTime.Now,
                            Course_Id = 2
                        },
                        new Exam
                        {
                            Name = "MVC Exam",
                            Type = ExamType.Exam,
                            Duration = 120,
                            Total_Marks = 10,
                            PassingPercentage = 50,
                            StartDate = DateTime.Now.AddDays(3),
                            CreatedAt = DateTime.Now,
                            Course_Id = 2
                        },
                        new Exam
                        {
                            Name = "Joins Exam",
                            Type = ExamType.Exam,
                            Duration = 120,
                            Total_Marks = 10,
                            PassingPercentage = 70,
                            StartDate = DateTime.Now.AddDays(5),
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
                             Text = "Define The Differance between Different Data Types.",
                             Mark = 50,
                             Exam_Id = 2
                         },
                         new Question
                         {
                              Text = "What is the entry point of a C# console application?",
                              Mark = 25,
                              Exam_Id = 3
                         },
                         new Question
                         {
                             Text = "Which datatype is used to store a single character in C#?",
                             Mark = 25,
                             Exam_Id = 3
                         },
                         new Question
                         {
                             Text = "Which lifecycle method registers a service as a single instance across the app?",
                             Mark = 10,
                             Exam_Id = 4
                         },
                         new Question
                         {
                             Text = "Implement a Book Store API.",
                             Mark = 10,
                             Exam_Id = 5
                         },
                         new Question
                         {
                             Text = "Which lifecycle method registers a service as a single instance across the app?",
                             Mark = 10,
                             Exam_Id = 6
                         },
                         new Question
                         {
                             Text = "Which SQL command to get all data from the table?",
                             Mark = 10,
                             Exam_Id = 7
                         },
                         new Question
                         {
                             Text = "What is Classes?",
                             Mark = 25,
                             Exam_Id = 8
                         },
                         new Question
                         {
                             Text = "What is Inheretance?",
                             Mark = 25,
                             Exam_Id = 8
                         },
                         new Question
                         {
                             Text = "Impelement ATM System",
                             Mark = 50,
                             Exam_Id = 9
                         },
                         new Question
                         {
                             Text = "What is Classes?",
                             Mark = 25,
                             Exam_Id = 10
                         },
                         new Question
                         {
                             Text = "What is Inheretance?",
                             Mark = 25,
                             Exam_Id = 10
                         },
                         new Question
                         {
                             Text = "What is View?",
                             Mark = 10,
                             Exam_Id = 10
                         },
                         new Question
                         {
                             Text = "Impelement any MVC project.",
                             Mark = 10,
                             Exam_Id = 11
                         },
                         new Question
                         {
                             Text = "What is View?",
                             Mark = 10,
                             Exam_Id = 12
                         },
                         new Question
                         {
                             Text = "What is Inner Join?",
                             Mark = 10,
                             Exam_Id = 13
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


                         new Choice { Text = "Main method", IsCorrect = true, Question_Id = 4 },
                         new Choice { Text = "Start method", IsCorrect = false, Question_Id = 4 },
                         new Choice { Text = "Page_Load", IsCorrect = false, Question_Id = 4 },


                         new Choice { Text = "string", IsCorrect = false, Question_Id = 5 },
                         new Choice { Text = "char", IsCorrect = true, Question_Id = 5 },
                         new Choice { Text = "int", IsCorrect = false, Question_Id = 5 },


                         new Choice { Text = "AddTransient", IsCorrect = false, Question_Id = 6 },
                         new Choice { Text = "AddScoped", IsCorrect = false, Question_Id = 6 },
                         new Choice { Text = "AddSingleton", IsCorrect = true, Question_Id = 6 },

                         new Choice { Text = "AddTransient", IsCorrect = false, Question_Id = 8 },
                         new Choice { Text = "AddScoped", IsCorrect = false, Question_Id = 8 },
                         new Choice { Text = "AddSingleton", IsCorrect = true, Question_Id = 8 },


                         new Choice { Text = "SELECT * FROM TableName", IsCorrect = true, Question_Id = 9 },
                         new Choice { Text = "GET * FROM TableName", IsCorrect = false, Question_Id = 9 },
                         new Choice { Text = "EXTRACT ALL FROM TableName", IsCorrect = false, Question_Id = 9 },
                         new Choice { Text = "FETCH ALL FROM TableName", IsCorrect = false, Question_Id = 9 },


                         new Choice { Text = "A tool used exclusively for performing arithmetic calculations.", IsCorrect = false, Question_Id = 10 },
                         new Choice { Text = "A storage variable that can only hold a single integer value at a time.", IsCorrect = false, Question_Id = 10 },
                         new Choice { Text = "A blueprint or template used to define the properties and behaviors of objects.", IsCorrect = true, Question_Id = 10 },


                         new Choice { Text = "A process that deletes parent classes from memory after a child object is created.", IsCorrect = false, Question_Id = 11 },
                         new Choice { Text = "A security protocol that encrypts variables to protect them from external hackers.", IsCorrect = false, Question_Id = 11 },
                         new Choice { Text = "A feature that allows a new class to adopt the attributes and methods of an existing class.", IsCorrect = true, Question_Id = 11 },


                         new Choice { Text = "A tool used exclusively for performing arithmetic calculations.", IsCorrect = false, Question_Id = 13 },
                         new Choice { Text = "A storage variable that can only hold a single integer value at a time.", IsCorrect = false, Question_Id = 13 },
                         new Choice { Text = "A blueprint or template used to define the properties and behaviors of objects.", IsCorrect = true, Question_Id = 13 },


                         new Choice { Text = "A process that deletes parent classes from memory after a child object is created.", IsCorrect = false, Question_Id = 14 },
                         new Choice { Text = "A security protocol that encrypts variables to protect them from external hackers.", IsCorrect = false, Question_Id = 14 },
                         new Choice { Text = "A feature that allows a new class to adopt the attributes and methods of an existing class.", IsCorrect = true, Question_Id = 14 },


                         new Choice { Text = "The component that directly manages database logic, queries, and data validation rules.", IsCorrect = false, Question_Id = 15 },
                         new Choice { Text = "The background routing system that intercepts HTTP requests and decides which function to execute.", IsCorrect = false, Question_Id = 15 },
                         new Choice { Text = "The visual interface or layout that displays data to the user and handles the presentation layer.", IsCorrect = true, Question_Id = 15 },


                         new Choice { Text = "The component that directly manages database logic, queries, and data validation rules.", IsCorrect = false, Question_Id = 17 },
                         new Choice { Text = "The background routing system that intercepts HTTP requests and decides which function to execute.", IsCorrect = false, Question_Id = 17 },
                         new Choice { Text = "The visual interface or layout that displays data to the user and handles the presentation layer.", IsCorrect = true, Question_Id = 17 },


                         new Choice { Text = "A command that deletes rows that do not have matching values in both tables.", IsCorrect = false, Question_Id = 18 },
                         new Choice { Text = "A process that merges two tables into a single table permanently by changing the database schema.", IsCorrect = false, Question_Id = 18 },
                         new Choice { Text = "A query that returns records only when there are matching values in both tables being joined.", IsCorrect = true, Question_Id = 18 }
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
                             Score = 25,
                             StartedAt = DateTime.Now.AddHours(-45),
                             SubmittedAt = DateTime.Now.AddHours(-15),
                             Student_Id = 2,
                             Exam_Id = 1
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 50,
                             StartedAt = DateTime.Now.AddHours(-2),
                             SubmittedAt = DateTime.Now.AddHours(-1),
                             Student_Id = 1,
                             Exam_Id = 2
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 50,
                             StartedAt = DateTime.Now.AddHours(-45),
                             SubmittedAt = DateTime.Now.AddHours(-15),
                             Student_Id = 2,
                             Exam_Id = 2
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 50,
                             StartedAt = DateTime.Now.AddHours(-2),
                             SubmittedAt = DateTime.Now.AddHours(-1),
                             Student_Id = 1,
                             Exam_Id = 3
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 50,
                             StartedAt = DateTime.Now.AddHours(-45),
                             SubmittedAt = DateTime.Now.AddHours(-15),
                             Student_Id = 2,
                             Exam_Id = 3
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 0,
                             StartedAt = DateTime.Now.AddHours(-2),
                             SubmittedAt = DateTime.Now.AddHours(-1),
                             Student_Id = 1,
                             Exam_Id = 4
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 10,
                             StartedAt = DateTime.Now.AddHours(-45),
                             SubmittedAt = DateTime.Now.AddHours(-15),
                             Student_Id = 2,
                             Exam_Id = 4
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 10,
                             StartedAt = DateTime.Now.AddHours(-2),
                             SubmittedAt = DateTime.Now.AddHours(-1),
                             Student_Id = 1,
                             Exam_Id = 5
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 10,
                             StartedAt = DateTime.Now.AddHours(-45),
                             SubmittedAt = DateTime.Now.AddHours(-15),
                             Student_Id = 2,
                             Exam_Id = 5
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 10,
                             StartedAt = DateTime.Now.AddHours(-2),
                             SubmittedAt = DateTime.Now.AddHours(-1),
                             Student_Id = 1,
                             Exam_Id = 6
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 10,
                             StartedAt = DateTime.Now.AddHours(-45),
                             SubmittedAt = DateTime.Now.AddHours(-15),
                             Student_Id = 2,
                             Exam_Id = 6
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 10,
                             StartedAt = DateTime.Now.AddHours(-2),
                             SubmittedAt = DateTime.Now.AddHours(-1),
                             Student_Id = 1,
                             Exam_Id = 7
                        },
                        new ExamAttempt
                        {
                             IsCompleted = true,
                             Score = 10,
                             StartedAt = DateTime.Now.AddHours(-45),
                             SubmittedAt = DateTime.Now.AddHours(-15),
                             Student_Id = 2,
                             Exam_Id = 7
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
                           AnswerdAt = DateTime.Now.AddHours(-1.5),
                           Student_Id = 1,
                           Question_Id = 4,
                           Choice_Id = 7
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.4),
                           Student_Id = 1,
                           Question_Id = 5,
                           Choice_Id = 11
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.4),
                           Student_Id = 1,
                           Question_Id = 6,
                           Choice_Id = 13
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.4),
                           Student_Id = 1,
                           Question_Id = 8,
                           Choice_Id = 18
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.4),
                           Student_Id = 1,
                           Question_Id = 9,
                           Choice_Id = 19
                       },
                      

                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.5),
                           Student_Id = 2,
                           Question_Id = 1,
                           Choice_Id = 1
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.4),
                           Student_Id = 2,
                           Question_Id = 2,
                           Choice_Id = 4
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.5),
                           Student_Id = 2,
                           Question_Id = 4,
                           Choice_Id = 7
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.4),
                           Student_Id = 2,
                           Question_Id = 5,
                           Choice_Id = 11
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.4),
                           Student_Id = 2,
                           Question_Id = 6,
                           Choice_Id = 15
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.4),
                           Student_Id = 2,
                           Question_Id = 8,
                           Choice_Id = 18
                       },
                       new StudentAnswer
                       {
                           AnswerdAt = DateTime.Now.AddHours(-1.4),
                           Student_Id = 2,
                           Question_Id = 9,
                           Choice_Id = 19
                       }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}