using System;

namespace MVCPatternExample
{
    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }

        public Student(string id, string name, string grade)
        {
            Id = id;
            Name = name;
            Grade = grade;
        }
    }

    public class StudentView
    {
        
        public void DisplayStudentDetails(string studentId, string studentName, string studentGrade)
        {
            Console.WriteLine("\n--- Student Details ---");
            Console.WriteLine($"ID: {studentId}");
            Console.WriteLine($"Name: {studentName}");
            Console.WriteLine($"Grade: {studentGrade}");
            Console.WriteLine("-----------------------");
        }
    }

    public class StudentController
    {
        private Student _model; // Reference to the Model (Student)
        private StudentView _view;  // Reference to the View (StudentView)

       
        public StudentController(Student model, StudentView view)
        {
            _model = model;
            _view = view;
        }

        // Methods to interact with the Model (update student data)
        public void SetStudentName(string name)
        {
            Console.WriteLine($"Controller: Updating student name from '{_model.Name}' to '{name}'");
            _model.Name = name;
        }

        public void SetStudentGrade(string grade)
        {
            Console.WriteLine($"Controller: Updating student grade from '{_model.Grade}' to '{grade}'");
            _model.Grade = grade;
        }

        // Methods to retrieve data from the Model
        public string GetStudentId()
        {
            return _model.Id;
        }

        public string GetStudentName()
        {
            return _model.Name;
        }

        public string GetStudentGrade()
        {
            return _model.Grade;
        }

        public void UpdateView()
        {
            Console.WriteLine("Controller: Instructing View to display updated details.");
            _view.DisplayStudentDetails(_model.Id, _model.Name, _model.Grade);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- MVC Pattern Example: Student Record Management ---\n");

            // 1. Create a Model object (Student)
            Console.WriteLine("Client: Creating initial Student Model...");
            Student student = new Student("S101", "Alice Smith", "A-");

            // 2. Create a View object (StudentView)
            Console.WriteLine("Client: Creating Student View...");
            StudentView view = new StudentView();

            // 3. Create a Controller object, linking Model and View
            Console.WriteLine("Client: Creating Student Controller, linking Model and View...");
            StudentController controller = new StudentController(student, view);

            // Initial display of student details
            Console.WriteLine("\nClient: Displaying initial student details:");
            controller.UpdateView();

            // Simulate updating student details via the Controller
            Console.WriteLine("\nClient: Simulating user interaction to update student name and grade...");
            controller.SetStudentName("Alice Wonderland");
            controller.SetStudentGrade("A+");

            // Display updated student details
            Console.WriteLine("\nClient: Displaying updated student details:");
            controller.UpdateView();

            // Simulate another update for a specific attribute
            Console.WriteLine("\nClient: Simulating user interaction to change only student grade...");
            controller.SetStudentGrade("B");

            Console.WriteLine("\nClient: Displaying final student details:");
            controller.UpdateView();


            Console.WriteLine("\n--- MVC Pattern Example Finished ---");
            Console.ReadKey(); // Keep console open until a key is pressed
        }
    }
}
