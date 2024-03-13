using System;
using System.Collections.Generic;

enum Department
{
    IT,
    HR,
    Finance
}

class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

class Employee : Person
{
    public int EmployeeId { get; }
    public double Salary { get; set; }
    public Department Department { get; set; }

    private static int totalEmployees = 0;

    public Employee(int employeeId, string firstName, string lastName, double salary, Department department)
    {
        EmployeeId = employeeId; //Unique id for each employee
        FirstName = firstName; //Employee's first name
        LastName = lastName; //Employee's last name
        Salary = salary; //Employee's salary
        Department = department; //Employee's department
        totalEmployees++; //How many employees are there in total
    }
    class Program
    {
        static void Main()
        {
            EmployeeManager manager = new EmployeeManager();

            //Static employees
            Employee emp1 = new Employee(1, "John", "Doe", 50000, Department.IT);
            Employee emp2 = new Employee(2, "Jane", "Smith", 60000, Department.HR);

            manager.AddEmployee(emp1);
            manager.AddEmployee(emp2);

            manager.DisplayAllEmployees();

            // Adding and removing employees based on user input
            while (true)
            {

                Console.WriteLine("\nChoose an action:");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Remove Employee");
                Console.WriteLine("3. Display All Employees");
                Console.WriteLine("4. Calculate Total Salary");
                Console.WriteLine("5. Exit");
                ConsoleKeyInfo keyInfo;
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1: //Ask for employee details
                        Console.WriteLine("Enter Employee ID:");
                        int empId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter First Name:");
                        string firstName = Console.ReadLine();
                        Console.WriteLine("Enter Last Name:");
                        string lastName = Console.ReadLine();
                        Console.WriteLine("Enter Salary:");
                        double salary = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("Enter Department (IT, HR, Finance):");
                        Department department = (Department)Enum.Parse(typeof(Department), Console.ReadLine());
                        Employee newEmployee = new Employee(empId, firstName, lastName, salary, department);
                        manager.AddEmployee(newEmployee);
                        break;

                    case ConsoleKey.D2: //Remove employee
                        Console.WriteLine("Enter Employee ID to remove:");
                        int empIdToRemove = Convert.ToInt32(Console.ReadLine());
                        manager.RemoveEmployee(empIdToRemove);
                        break;

                    case ConsoleKey.D3: //Display all registered employees
                        manager.DisplayAllEmployees();
                        break;

                    case ConsoleKey.D4: //Calculate the salaries in total
                        manager.CalculateTotalSalary();
                        break;

                    case ConsoleKey.D5: //Exit the program
                        Console.WriteLine("See you next time.");
                        Environment.Exit(0);
                        return;

                    default: //If user input is not in the choices
                        Console.WriteLine("Invalid choice. Please only press the number of the corresponding keys.");
                        break;
                }
            }
        }
    }

    public static int GetTotalEmployees() //Know how many employees are there in total
    {
        return totalEmployees;
    }
}

interface IManager
{
    void AssignEmployeeToDepartment(Employee employee, Department department);
}

class EmployeeManager : IManager
{
    private List<Employee> employees = new List<Employee>(); //Stores employee data and checks input

    public void AddEmployee(Employee employee)
    {
        if (String.IsNullOrWhiteSpace(employee.FirstName) || String.IsNullOrWhiteSpace(employee.LastName) || employee.FirstName.Any(char.IsDigit) || employee.LastName.Any(char.IsDigit))
        {
            Console.WriteLine("One or more input is invalid. Please try again.");
            Console.WriteLine("Reminder: Only use letters.");
            Thread.Sleep(1000);
            return;
        }
        else if (!employees.Exists(e => e.EmployeeId == employee.EmployeeId)) //Checks if employee exists
        {
            employees.Add(employee);
            Console.WriteLine($"Employee {employee.FirstName} {employee.LastName} added successfully."); //Notes that employee has been added successfully
        }
        else
        {
            Console.WriteLine($"Cannot enter employee with the same ID."); //Notes that employee already exists
        }
    }

    public void RemoveEmployee(int employeeId)
    {
        var employeeToRemove = employees.Find(e => e.EmployeeId == employeeId); //Checks if employee exists
        if (employeeToRemove != null)
        {
            employees.Remove(employeeToRemove);
            Console.WriteLine($"Employee {employeeToRemove.FirstName} {employeeToRemove.LastName} removed successfully."); //Notes that employee has been removed
        }
        else
        {
            Console.WriteLine($"Employee with ID {employeeId} not found."); //Notes that employee is non-existent
        }
    }

    public void DisplayAllEmployees() //Displays all employee datas by showing their ID, First Name, Last Name, Salary, and Department
    {
        foreach (var employee in employees)
        {
            Console.WriteLine($"ID: {employee.EmployeeId}, Name: {employee.FirstName} {employee.LastName}, Salary: {employee.Salary}, Department: {employee.Department}");
        }
    }

    public void CalculateTotalSalary() //Calculates how much salary the employees receive in total
    {
        double totalSalary = employees.Sum(e => e.Salary);
        Console.WriteLine($"Total Salary of all employees: {totalSalary}");
    }

    public void AssignEmployeeToDepartment(Employee employee, Department department) //Shows which department the employee has been assigned to
    {
        if (Enum.IsDefined(typeof(Department), department))
        {
            employee.Department = department;
            Console.WriteLine($"Employee {employee.FirstName} {employee.LastName} assigned to {department} department.");
        }
        else
        {
            Console.WriteLine($"Department: {department} does not exist. Please choose a valid department (IT, HR, Finance).");
        }
    }
}

