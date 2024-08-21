using System;
using System.Data.SqlClient;

namespace School_management_system_final_last
{
    class Program
    {
        static string connectionString = @"Data Source=DESKTOP-R7ILTMI\SQLEXPRESS;Initial Catalog=School_Management_System;Integrated Security=True";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("School Management System");
                Console.WriteLine("1. Manage Students");
                Console.WriteLine("2. Manage Teachers");
                Console.WriteLine("3. Manage Courses");
                Console.WriteLine("4. Manage Grades");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageStudents();
                        break;
                    case "2":
                        ManageTeachers();
                        break;
                    case "3":
                        ManageCourses();
                        break;
                    case "4":
                        ManageGrades();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        // Manage Students
        static void ManageStudents()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Students");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View Students");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Back");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        ViewStudents();
                        break;
                    case "3":
                        UpdateStudent();
                        break;
                    case "4":
                        DeleteStudent();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddStudent()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    Console.Write("Enter Student Name: ");
                    string name = Console.ReadLine();
                    if (string.IsNullOrEmpty(name))
                    {
                        Console.WriteLine("Student name cannot be empty.");
                        return;
                    }

                    Console.Write("Enter Date of Birth (yyyy-MM-dd): ");
                    string dob = Console.ReadLine();
                    if (!DateTime.TryParseExact(dob, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime dateOfBirth))
                    {
                        Console.WriteLine("Invalid date format. Please enter the date in yyyy-MM-dd format.");
                        return;
                    }

                    Console.Write("Enter Gender: ");
                    string gender = Console.ReadLine();
                    if (string.IsNullOrEmpty(gender))
                    {
                        Console.WriteLine("Gender cannot be empty.");
                        return;
                    }

                    Console.Write("Enter Class: ");
                    string studentClass = Console.ReadLine();
                    if (string.IsNullOrEmpty(studentClass))
                    {
                        Console.WriteLine("Class cannot be empty.");
                        return;
                    }

                    Console.Write("Enter Contact Info: ");
                    string contactInfo = Console.ReadLine();

                    var query = "INSERT INTO Students (Name, DateOfBirth, Gender, Class, ContactInfo) VALUES (@Name, @DateOfBirth, @Gender, @Class, @ContactInfo)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                        command.Parameters.AddWithValue("@Gender", gender);
                        command.Parameters.AddWithValue("@Class", studentClass);
                        command.Parameters.AddWithValue("@ContactInfo", contactInfo);

                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("Student added successfully!");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }




        static void ViewStudents()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Students";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["StudentID"]}, Name: {reader["Name"]}, DOB: {reader["DateOfBirth"]}, Gender: {reader["Gender"]}, Class: {reader["Class"]}, Contact: {reader["ContactInfo"]}");
                    }
                }
            }

            // Yahan pause karne ke liye aik line add karain taake user results dekh sake
            Console.WriteLine("\nPress any key to return to the previous menu.");
            Console.ReadKey();
        }


        static void UpdateStudent()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Student ID to Update: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter New Name (leave blank to keep current): ");
                string name = Console.ReadLine();

                Console.Write("Enter New Date of Birth (yyyy-mm-dd, leave blank to keep current): ");
                string dob = Console.ReadLine();

                Console.Write("Enter New Gender (leave blank to keep current): ");
                string gender = Console.ReadLine();

                Console.Write("Enter New Class (leave blank to keep current): ");
                string studentClass = Console.ReadLine();

                Console.Write("Enter New Contact Info (leave blank to keep current): ");
                string contactInfo = Console.ReadLine();

                var query = "UPDATE Students SET Name = ISNULL(NULLIF(@Name, ''), Name), DateOfBirth = ISNULL(NULLIF(@DateOfBirth, ''), DateOfBirth), Gender = ISNULL(NULLIF(@Gender, ''), Gender), Class = ISNULL(NULLIF(@Class, ''), Class), ContactInfo = ISNULL(NULLIF(@ContactInfo, ''), ContactInfo) WHERE StudentID = @StudentID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@DateOfBirth", string.IsNullOrEmpty(dob) ? (object)DBNull.Value : DateTime.Parse(dob));
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@Class", studentClass);
                    command.Parameters.AddWithValue("@ContactInfo", contactInfo);

                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Student updated successfully!");
            }
        }

        static void DeleteStudent()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Student ID to Delete: ");
                int id = int.Parse(Console.ReadLine());

                var query = "DELETE FROM Students WHERE StudentID = @StudentID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", id);
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Student deleted successfully!");
            }
        }

        // Manage Teachers
        static void ManageTeachers()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Teachers");
                Console.WriteLine("1. Add Teacher");
                Console.WriteLine("2. View Teachers");
                Console.WriteLine("3. Update Teacher");
                Console.WriteLine("4. Delete Teacher");
                Console.WriteLine("5. Back");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTeacher();
                        break;
                    case "2":
                        ViewTeachers();
                        break;
                    case "3":
                        UpdateTeacher();
                        break;
                    case "4":
                        DeleteTeacher();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddTeacher()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Teacher Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Subject: ");
                string subject = Console.ReadLine();

                Console.Write("Enter Contact Info: ");
                string contactInfo = Console.ReadLine();

                var query = "INSERT INTO Teachers (Name, Subject, ContactInfo) VALUES (@Name, @Subject, @ContactInfo)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Subject", subject);
                    command.Parameters.AddWithValue("@ContactInfo", contactInfo);

                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Teacher added successfully!");
            }
        }

        static void ViewTeachers()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Teachers";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["TeacherID"]}, Name: {reader["Name"]}, Subject: {reader["Subject"]}, Contact: {reader["ContactInfo"]}");
                    }
                }
            }
            Console.WriteLine("\nPress any key to return to the previous menu.");
            Console.ReadKey();
        }

                static void UpdateTeacher()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Teacher ID to Update: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter New Name (leave blank to keep current): ");
                string name = Console.ReadLine();

                Console.Write("Enter New Subject (leave blank to keep current): ");
                string subject = Console.ReadLine();

                Console.Write("Enter New Contact Info (leave blank to keep current): ");
                string contactInfo = Console.ReadLine();

                var query = "UPDATE Teachers SET Name = ISNULL(NULLIF(@Name, ''), Name), Subject = ISNULL(NULLIF(@Subject, ''), Subject), ContactInfo = ISNULL(NULLIF(@ContactInfo, ''), ContactInfo) WHERE TeacherID = @TeacherID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeacherID", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Subject", subject);
                    command.Parameters.AddWithValue("@ContactInfo", contactInfo);

                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Teacher updated successfully!");
            }
        }

        static void DeleteTeacher()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Teacher ID to Delete: ");
                int id = int.Parse(Console.ReadLine());

                var query = "DELETE FROM Teachers WHERE TeacherID = @TeacherID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeacherID", id);
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Teacher deleted successfully!");
            }
        }

        // Manage Courses
        static void ManageCourses()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Courses");
                Console.WriteLine("1. Add Course");
                Console.WriteLine("2. View Courses");
                Console.WriteLine("3. Update Course");
                Console.WriteLine("4. Delete Course");
                Console.WriteLine("5. Back");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCourse();
                        break;
                    case "2":
                        ViewCourses();
                        break;
                    case "3":
                        UpdateCourse();
                        break;
                    case "4":
                        DeleteCourse();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddCourse()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Course Name: ");
                string courseName = Console.ReadLine();

                Console.Write("Enter Teacher ID: ");
                int teacherID = int.Parse(Console.ReadLine());

                var query = "INSERT INTO Courses (CourseName, TeacherID) VALUES (@CourseName, @TeacherID)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseName", courseName);
                    command.Parameters.AddWithValue("@TeacherID", teacherID);

                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Course added successfully!");
            }
        }

        static void ViewCourses()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Courses";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["CourseID"]}, Name: {reader["CourseName"]}, Teacher ID: {reader["TeacherID"]}");
                    }
                }
            }
            Console.WriteLine("\nPress any key to return to the previous menu.");
            Console.ReadKey();
        }

        static void UpdateCourse()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Course ID to Update: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter New Course Name (leave blank to keep current): ");
                string courseName = Console.ReadLine();

                Console.Write("Enter New Teacher ID (leave blank to keep current): ");
                string teacherID = Console.ReadLine();

                var query = "UPDATE Courses SET CourseName = ISNULL(NULLIF(@CourseName, ''), CourseName), TeacherID = ISNULL(NULLIF(@TeacherID, ''), TeacherID) WHERE CourseID = @CourseID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", id);
                    command.Parameters.AddWithValue("@CourseName", courseName);
                    command.Parameters.AddWithValue("@TeacherID", string.IsNullOrEmpty(teacherID) ? (object)DBNull.Value : int.Parse(teacherID));

                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Course updated successfully!");
            }
        }

        static void DeleteCourse()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Course ID to Delete: ");
                int id = int.Parse(Console.ReadLine());

                var query = "DELETE FROM Courses WHERE CourseID = @CourseID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", id);
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Course deleted successfully!");
            }
        }

        // Manage Grades
        static void ManageGrades()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Grades");
                Console.WriteLine("1. Add Grade");
                Console.WriteLine("2. View Grades");
                Console.WriteLine("3. Update Grade");
                Console.WriteLine("4. Delete Grade");
                Console.WriteLine("5. Back");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddGrade();
                        break;
                    case "2":
                        ViewGrades();
                        break;
                    case "3":
                        UpdateGrade();
                        break;
                    case "4":
                        DeleteGrade();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddGrade()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Student ID: ");
                int studentID = int.Parse(Console.ReadLine());

                Console.Write("Enter Course ID: ");
                int courseID = int.Parse(Console.ReadLine());

                Console.Write("Enter Grade: ");
                string grade = Console.ReadLine();

                var query = "INSERT INTO Grades (StudentID, CourseID, Grade) VALUES (@StudentID, @CourseID, @Grade)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentID);
                    command.Parameters.AddWithValue("@CourseID", courseID);
                    command.Parameters.AddWithValue("@Grade", grade);

                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Grade added successfully!");
            }
        }

        static void ViewGrades()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Grades";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["GradeID"]}, Student ID: {reader["StudentID"]}, Course ID: {reader["CourseID"]}, Grade: {reader["Grade"]}");
                    }
                }
            }
            Console.WriteLine("\nPress any key to return to the previous menu.");
            Console.ReadKey();
        }

        static void UpdateGrade()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Grade ID to Update: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter New Grade (leave blank to keep current): ");
                string grade = Console.ReadLine();

                var query = "UPDATE Grades SET Grade = ISNULL(NULLIF(@Grade, ''), Grade) WHERE GradeID = @GradeID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GradeID", id);
                    command.Parameters.AddWithValue("@Grade", grade);

                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Grade updated successfully!");
            }
        }

        static void DeleteGrade()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter Grade ID to Delete: ");
                int id = int.Parse(Console.ReadLine());

                var query = "DELETE FROM Grades WHERE GradeID = @GradeID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GradeID", id);
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Grade deleted successfully!");
            }
        }
    }
}

    
