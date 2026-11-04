using Microsoft.EntityFrameworkCore;
using ORM_Assignment3.Context;
using ORM_Assignment3.Models;

namespace ORM_Assignment3
{
    internal class Program
    {
        #region CreateMethods
        // Add an instructor: Insert a new instructor and print the generated Id.
        static int AddInstructor(DBContext dbContext,
            string name,
            string email,
            string bio,
            DateTime joinDate)
        {
            var instructor01 = new Instructor
            {
                FullName = name,
                Email = email,
                Bio = bio,
                JoinDate = joinDate
            };

            Console.WriteLine("\nBefore Add:");
            Console.WriteLine("State: " + dbContext.Entry(instructor01).State);
            Console.WriteLine("Instructor Id: " + instructor01.InstructorId);

            dbContext.Add(instructor01);

            Console.WriteLine("\nAfter Add:");
            Console.WriteLine("State: " + dbContext.Entry(instructor01).State);
            Console.WriteLine("Instructor Id: " + instructor01.InstructorId);

            dbContext.SaveChanges();

            Console.WriteLine("\nAfter SaveChanges:");
            Console.WriteLine("State: " + dbContext.Entry(instructor01).State);
            Console.WriteLine("Instructor Id: " + instructor01.InstructorId);

            return instructor01.InstructorId;
        }

        //Add a course: Insert a course for an existing instructor and an existing category.
        //Check that both exist first, otherwise print a message.
        static int AddCourse(DBContext dbContext,
            string title,
            string description,
            decimal price,
            Level level,
            int instructorId,
            int categoryId)
        {
            var instructor = dbContext.Instructors.Find(instructorId);
            if (instructor == null)
            {
                Console.WriteLine("Cannot add course,Instructor does not exist.");
                return 0;
            }

            var category = dbContext.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine("Cannot add course: Category does not exist.");
                return 0;
            }

            var course = new Course
            {
                Title = title,
                Description = description,
                Price = price,
                Level = level,
                IsPublished = false,
                PublishedAt = null,
                InstructorId = instructorId,
                CategoryId = categoryId
            };

            dbContext.Courses.Add(course);
            dbContext.SaveChanges();

            Console.WriteLine("\nCourse added successfully!");
            Console.WriteLine("Course Id: " + course.CourseId);

            return course.CourseId;
        }

        //Add several students: Insert three students using one AddRange and one SaveChanges.
        //Print their generated Ids.
        static List<int> AddStudents(DBContext dbContext,
            List<Student> students)
        {
            dbContext.Students.AddRange(students);
            dbContext.SaveChanges();

            Console.WriteLine("\nStudents added successfully:");

            foreach (var student in students)
            {
                Console.WriteLine($"Student Name: {student.FullName}, Id: {student.StudentId}");
            }
            return students.Select(s => s.StudentId).ToList();
        }

        //Enroll a student: The student and the course must exist,
        //the course must be published, and the student must not be enrolled already.
        //Otherwise print why the enrollment was refused.
        static void EnrollStudent(DBContext dbContext,
            int studentId,
            int courseId)
        {
            var student = dbContext.Students.Find(studentId);
            if (student == null)
            {
                Console.WriteLine("\nEnrollment refused, Student does not exist.");
                return;
            }

            var course = dbContext.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine("\nEnrollment refused, Course does not exist.");
                return;
            }

            if (!course.IsPublished)
            {
                Console.WriteLine("\nEnrollment refused, Course is not published.");
                return;
            }

            bool alreadyEnrolled = dbContext.Enrollments.Any(e =>
                e.StudentId == studentId && e.CourseId == courseId);
            if (alreadyEnrolled)
            {
                Console.WriteLine("\nEnrollment refused, Student is already enrolled.");
                return;
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                Progress = 0,
                Status = EnrollmentStatus.Active,
                FinalGrade = null
            };

            dbContext.Enrollments.Add(enrollment);
            dbContext.SaveChanges();
            Console.WriteLine("\nStudent enrolled successfully!");
        }
        #endregion

        #region ReadMethods

        //Find a course: Get a course by Id using Find, then by title using FirstOrDefault.
        //Print "not found" when the result is null.
        static void FindCourse(DBContext dbContext, int courseId, string courseTitle)
        {
            var courseById = dbContext.Courses.Find(courseId);
            if (courseById == null)
            {
                Console.WriteLine("Course with this Id not found.");
            }
            else
            {
                Console.WriteLine("\nCourse found by Id:");
                Console.WriteLine($"Id: {courseById.CourseId}, Title: {courseById.Title}");
            }

            var courseByTitle = dbContext.Courses.FirstOrDefault(
                c => c.Title == courseTitle
            );
            if (courseByTitle == null)
            {
                Console.WriteLine("Course with this Title not found.");
            }
            else
            {
                Console.WriteLine("\nCourse found by Title:");
                Console.WriteLine($"Id: {courseByTitle.CourseId}, Title: {courseByTitle.Title}");
            }
        }


        //Filter courses: Courses priced between two values,
        //ordered by price descending.Print only the title, level and price.
        static void FilterCourses(DBContext dbContext, decimal minPrice, decimal maxPrice)
        {
            var courses = dbContext.Courses
                .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
                .OrderByDescending(c => c.Price)
                .ToList();

            if (courses.Count == 0)
            {
                Console.WriteLine("No courses found.");
                return;
            }

            foreach (var course in courses)
            {
                Console.WriteLine(
                    $"Title: {course.Title}, Level: {course.Level}, Price: {course.Price}"
                );
            }
        }


        // Sub-categories: Print the direct sub-categories of a given category,
        // then print the top-level categories.
        static void PrintCategories(DBContext dbContext, int categoryId)
        {
            var subCategories = dbContext.Categories
                .Where(c => c.ParentCategoryId == categoryId)
                .ToList();

            Console.WriteLine("\nDirect Sub-Categories:");
            if (subCategories.Count == 0)
            {
                Console.WriteLine("No sub-categories found.");
            }
            else
            {
                foreach (var category in subCategories)
                {
                    Console.WriteLine($"Id: {category.CategoryId}, Name: {category.Name}");
                }
            }

            var topLevelCategories = dbContext.Categories
                .Where(c => c.ParentCategoryId == null)
                .ToList();

            Console.WriteLine("\nTop-Level Categories:");
            foreach (var category in topLevelCategories)
            {
                Console.WriteLine($"Id: {category.CategoryId}, Name: {category.Name}");
            }
        }


        //Course roster: Print how many students are enrolled in a course, then their names.
        //Do it without joins: get the student Ids from the enrollments first,
        //then get the students with Contains.
        static void PrintCourseRoster(DBContext dbContext, int courseId)
        {
            var studentIds = dbContext.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId)
                .ToList();

            Console.WriteLine($"\nNumber of enrolled students: {studentIds.Count}");
            if (studentIds.Count == 0)
            {
                Console.WriteLine("No students enrolled in this course.");
                return;
            }

            var students = dbContext.Students
                .Where(s => studentIds.Contains(s.StudentId))
                .ToList();
            Console.WriteLine("Enrolled Students:");
            foreach (var student in students)
            {
                Console.WriteLine(student.FullName);
            }
        }

        #endregion

        #region UpdateMethods

        // Change a price: Load a course, change its price, and save
        static void ChangeCoursePrice(DBContext dbContext, int courseId, decimal newPrice)
        {
            var course = dbContext.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine("\nCourse not found.");
                return;
            }

            course.Price = newPrice;
            dbContext.SaveChanges();

            Console.WriteLine("\nCourse price updated successfully!");
            Console.WriteLine($"New Price: {course.Price}");
        }

        // Update progress: Find the enrollment by its composite key and set the new progress.
        // At 100 set the status to Completed. Reject values outside 0–100.
        static void UpdateProgress(DBContext dbContext,
            int studentId,
            int courseId,
            decimal newProgress)
        {
            if (newProgress < 0 || newProgress > 100)
            {
                Console.WriteLine("\nInvalid progress! It must be between 0 and 100.");
                return;
            }

            var enrollment = dbContext.Enrollments.Find(studentId, courseId);
            if (enrollment == null)
            {
                Console.WriteLine("\nEnrollment not found.");
                return;
            }

            enrollment.Progress = newProgress;
            if (newProgress == 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
            }

            dbContext.SaveChanges();
            Console.WriteLine("\nEnrollment progress updated successfully!");
            Console.WriteLine($"Progress: {enrollment.Progress}%");
            Console.WriteLine($"Status: {enrollment.Status}");
        }

        // Publish a course: Set the published flag and the published date
        static void PublishCourse(DBContext dbContext, int courseId)
        {
            var course = dbContext.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine("\nCourse not found.");
                return;
            }

            course.IsPublished = true;
            course.PublishedAt = DateTime.Now;

            dbContext.SaveChanges();
            Console.WriteLine("\nCourse published successfully!");
            Console.WriteLine($"Published: {course.IsPublished}");
            Console.WriteLine($"Published At: {course.PublishedAt}");
        }

        #endregion

        #region DeleteMethods

        // Unenroll: Delete an enrollment using its composite key.
        static void UnenrollStudent(DBContext dbContext, int studentId, int courseId)
        {
            var enrollment = dbContext.Enrollments.Find(studentId, courseId);
            if (enrollment == null)
            {
                Console.WriteLine("\nEnrollment not found.");
                return;
            }

            dbContext.Enrollments.Remove(enrollment);
            dbContext.SaveChanges();
            Console.WriteLine("\nStudent unenrolled successfully!");
        }


        // Delete a category: First try to remove a category that still has sub-categories
        // and catch the DbUpdateException, then print its message.
        // After that, write a safe version that checks for sub-categories before deleting.
        static void TryDeleteCategory(DBContext dbContext, int categoryId)
        {
            var category = dbContext.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine("\nCategory not found.");
                return;
            }

            dbContext.Categories.Remove(category);
            try
            {
                dbContext.SaveChanges();
                Console.WriteLine("\nCategory deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("\nCannot delete this category.");
                Console.WriteLine("Error: " + ex.Message);
                dbContext.Entry(category).State = EntityState.Unchanged;
            }
        }

        static void SafeDeleteCategory(DBContext dbContext, int categoryId)
        {
            var category = dbContext.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine("\nCategory not found.");
                return;
            }

            bool hasSubCategories = dbContext.Categories
                .Any(c => c.ParentCategoryId == categoryId);

            if (hasSubCategories)
            {
                Console.WriteLine("\nCannot delete category because it has sub-categories.");
                return;
            }

            dbContext.Categories.Remove(category);
            dbContext.SaveChanges();

            Console.WriteLine("\nCategory deleted successfully!");
        }


        // Delete a course: Delete a course that has enrollments.
        // Print the number of its enrollments before and after,
        // and explain in a comment why they were removed.
        static void DeleteCourse(DBContext dbContext, int courseId)
        {
            var course = dbContext.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine("\nCourse not found.");
                return;
            }

            int enrollmentsBefore = dbContext.Enrollments
                .Count(e => e.CourseId == courseId);

            Console.WriteLine($"\nEnrollments before deletion: {enrollmentsBefore}");
            dbContext.Courses.Remove(course);
            dbContext.SaveChanges();

            int enrollmentsAfter = dbContext.Enrollments
                .Count(e => e.CourseId == courseId);
            Console.WriteLine($"Enrollments after deletion: {enrollmentsAfter}");
            Console.WriteLine("Course deleted successfully!");

        }

        #endregion

        static void Main(string[] args)
        {
            using DBContext dbContext = new DBContext();

            #region CreateOperations
            #region Add an instructor
            //int instructorId = AddInstructor(
            //    dbContext,
            //    "NewInstructor1",
            //    "NewInstructor1@gmail.com",
            //    "Bio for NewInstructor1",
            //    DateTime.Now
            //); 
            #endregion

            #region Add a course
            //int courseId = AddCourse(
            //    dbContext,
            //    "ASP.NET Core",
            //    "Introduction to ASP.NET Core",
            //    600,
            //    Level.Beginner,
            //    2,
            //    1
            //); 
            #endregion

            #region Add several students
            //var students = new List<Student>
            // {
            //     new Student
            //     {
            //         FullName = "Student One",
            //         Email = "student1@gmail.com",
            //         DOB = new DateTime(2003, 5, 10)
            //     },

            //     new Student
            //     {
            //         FullName = "Student Two",
            //         Email = "student2@gmail.com",
            //         DOB = new DateTime(2004, 8, 15)
            //     },

            //     new Student
            //     {
            //         FullName = "Student Three",
            //         Email = "student3@gmail.com",
            //         DOB = new DateTime(2003, 12, 20)
            //     }
            // };
            //List<int> studentIds = AddStudents(dbContext, students);

            #endregion

            #region Enroll a student
            //EnrollStudent(dbContext, 3, 1);
            #endregion

            #endregion

            #region ReadOperations

            #region Find a Course
            //FindCourse(dbContext, 1, "Course1");
            #endregion

            #region Filter Courses
            //FilterCourses(dbContext, 300, 1000);
            #endregion

            #region Sub-Categories
            //PrintCategories(dbContext, 1); 
            #endregion

            #region Course Roster
            //PrintCourseRoster(dbContext, 1); 
            #endregion

            #endregion

            #region UpdateOperations

            #region Change a price
            //ChangeCoursePrice(dbContext, 1, 750);
            #endregion

            #region Update progress
            //UpdateProgress(dbContext, 1, 1, 100);
            #endregion

            #region Publish a course
            //PublishCourse(dbContext, 2);
            #endregion

            #endregion

            #region DeleteOperations

            #region Unenroll
            //UnenrollStudent(dbContext, 1, 1);
            #endregion

            #region Delete Category
            //TryDeleteCategory(dbContext, 1);

            //SafeDeleteCategory(dbContext, 1);
            #endregion

            #region Delete Course
            //DeleteCourse(dbContext, 1);
            #endregion

            #endregion

        }
    }
}
