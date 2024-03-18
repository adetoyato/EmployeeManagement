using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

enum Department
{
    IT,
    HR,
    Finance,
    Marketing,
    Production
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
            Employee emp2 = new Employee(2, "William", "Doe", 60000, Department.HR);

            manager.AddEmployee(emp1);
            manager.AddEmployee(emp2);

            manager.DisplayAllEmployees();

            // Adding and removing employees based on user input
            while (true)
            {

                Console.WriteLine("\nPress the corresponding number according to the action you wish to take: ");
                Console.WriteLine("[1] Add Employee\n[2] Remove Employee\n[3] Display All Employees\n[4] Calculate Total Salary\n[5] Exit");
                ConsoleKeyInfo keyInfo;
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1: //Ask for employee details
                        int EmpIdNum = 0;
                        bool ValidEmpId = false;
                        do
                        {
                        Console.WriteLine("Enter Employee ID:");
                        string EmpId = Console.ReadLine();
                        
                            if (!int.TryParse(EmpId, out EmpIdNum)) //Checks if the string entered is only numbers. If there are letters, prompt message.
                            {
                                Console.WriteLine("Please enter only numbers.");
                                continue;
                            }
                            if (EmpIdNum < 0)
                            {
                                Console.WriteLine("Please enter only a positive integer."); //Checks if the input digit is a negative number. If yes, prompt message.
                                continue;
                            }
                            else
                            {
                                ValidEmpId = true;
                            }
                        } while (!ValidEmpId) ;
                        Console.WriteLine("Enter First Name:"); //Names gets validated under AddEmployee();
                        string FirstName = Console.ReadLine();
                        Console.WriteLine("Enter Last Name:");
                        string LastName = Console.ReadLine();

                        //Salary validation
                        double salary = 0;
                        bool ValidSalary = false;
                        do
                        {
                            Console.WriteLine("Enter Salary:");
                            string salaryInput = Console.ReadLine();
                            if (!double.TryParse(salaryInput, out salary)) //Checks if the string entered is only numbers. If there are letters, prompt message.
                            {
                                Console.WriteLine("Please enter only numbers.");
                                continue;
                            }

                            if (salary < 0)
                            {
                                Console.WriteLine("Please enter only a positive integer."); //Checks if the input digit is a negative number. If yes, prompt message.
                                continue;
                            }
                            else
                            {
                                ValidSalary = true;
                            }
                        } while (!ValidSalary);

                        Department department;
                        bool ValidDepartment = false;
                        do
                        {
                            Console.WriteLine("Enter Department (HR, IT, Finance, Marketing, Production) to assign employee to: ");
                            if (Enum.TryParse(Console.ReadLine(), true, out department)) // Checks if the entered department is available
                            {
                                if (Enum.IsDefined(typeof(Department), department))
                                {
                                    Employee NewEmployee = new Employee(EmpIdNum, FirstName, LastName, salary, department);
                                    manager.AddEmployee(NewEmployee);
                                }
                                else
                                {
                                    Console.WriteLine($"Department: {department} does not exist. Please choose a valid department (HR, IT, Finance, Marketing, Production).");
                                }
                            }
                            else
                            {
                                ValidDepartment = true;
                            }
                        } while (!ValidDepartment);
                        break;

                    case ConsoleKey.D2: // Remove employee
                        int IdNum = 0;
                        bool FoundEmpId = false;
                        do
                        {
                            Console.WriteLine("Enter Employee ID to remove:");
                            string IdInput = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(IdInput))  //Checks if input is null or white space
                            {
                                Console.WriteLine("Input is empty. Please try again.");
                                continue;
                            }
                            if (!int.TryParse(IdInput, out IdNum)) //Checks if the string entered is only numbers. If there are letters, prompt message.
                            {
                                Console.WriteLine("Please enter only numbers.");
                                continue;
                            }
                            if (IdNum < 0)
                            {
                                Console.WriteLine("Please enter only a positive integer."); //Checks if the input digit is a negative number. If yes, prompt message.
                                continue;
                            }
                            int EmpIdToRemove;
                            if (!int.TryParse(IdInput, out EmpIdToRemove))
                            {
                                Console.WriteLine("Invalid input. Please enter a valid integer ID.");
                                continue;
                            }
                            else
                            {
                                FoundEmpId = true;
                            }
                        manager.RemoveEmployee(EmpIdToRemove);
                        } while (!FoundEmpId);
                        break;

                    case ConsoleKey.D3: //Display all registered employees
                        manager.DisplayAllEmployees();
                        break;

                    case ConsoleKey.D4: //Calculate the salaries in total
                        manager.CalculateTotalSalary();
                        break;

                    case ConsoleKey.D5: //Exit the program
                        Console.WriteLine("Shutting down system...");
                        Thread.Sleep(1000);
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

class EmployeeManager
{
    private List<Employee> employees = new List<Employee>(); //Stores employee data and checks input

    public void AddEmployee(Employee employee)
    {
        if (String.IsNullOrWhiteSpace(employee.FirstName) || String.IsNullOrWhiteSpace(employee.LastName) || employee.FirstName.Any(char.IsDigit) || employee.LastName.Any(char.IsDigit)) //Checks if input is valid (if its null, or if there's a digit)
        {
            Console.WriteLine("Error: Invalid name.");
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

    public void RemoveEmployee(int EmployeeId)
    {
        var EmployeeToRemove = employees.Find(e => e.EmployeeId == EmployeeId); //Checks if employee exists
        if (EmployeeToRemove != null)
        {
            employees.Remove(EmployeeToRemove);
            Console.WriteLine($"Employee {EmployeeToRemove.FirstName} {EmployeeToRemove.LastName} removed successfully."); //Notes that employee has been removed
        }
        else
        {
            Console.WriteLine($"Employee with ID {EmployeeId} not found."); //Notes that employee is non-existent
        }
    }

    public void DisplayAllEmployees() //Displays all employee data by showing their ID, First Name, Last Name, Salary, and Department
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
}

