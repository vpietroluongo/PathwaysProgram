using System;
using System.Net.NetworkInformation;

namespace EmployeeBonus
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quitProgram = false;
            const int arraySize = 25;
            Employee[] employees = new Employee[arraySize];
            string fileName = "employees.txt";

            //loop until Q is entered and quitProgram flag changes
            do
            {  
                Console.WriteLine("Choose an option:");
                Console.WriteLine("L - Load list of employees");
                Console.WriteLine("S - Save list of employees");
                Console.WriteLine("C - Add an employee record");
                Console.WriteLine("R - Print list of all employee records");
                Console.WriteLine("U - Update information for an employee");
                Console.WriteLine("D - Delete an employee record");
                Console.WriteLine("Q - Quit the program"); 

                string input = (Console.ReadLine()).ToUpper();

                switch (input)
                {
                    case "L":
                        Console.WriteLine("In L area");
                        employees = FileToArray(fileName, arraySize);
                        break;
                    case "S":
                        Console.WriteLine("In S area");
                        ArrayToFile(employees, fileName);
                        break;
                    case "C":
                        Console.WriteLine("In C area");
                        CreateEmployee(employees);
                        break;
                    case "R":
                        Console.WriteLine("In R area");
                        PrintEmployees(employees);
                        break;
                    case "U":
                        Console.WriteLine("In U area");
                        UpdateEmployee(employees);
                        break;
                    case "D":
                        Console.WriteLine("In D area");
                        DeleteEmployee(employees);
                        break;
                    case "Q":
                        Console.WriteLine("In Q area");
                        quitProgram = true;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid option.");
                        break;
                } //end switch
            } while(!quitProgram);
        }  //end Main method



         static Employee[] FileToArray(string file, int arraySize)
        {
            bool emptyFile = true;
            string lineRead = "";
            int i = 0;
            int lineCounter = 0;
            Employee[] employees = new Employee[arraySize];

            try
            {
                using (StreamReader reader = File.OpenText(file))
                {

                    while ((lineRead = reader.ReadLine()) != null)
                    {
                        emptyFile = false;
                        Console.WriteLine($"{lineRead} i: {i} lineCounter: {lineCounter}");
                        switch (lineCounter)
                        {
                            case 0:
                                employees[i] = new Employee();
                                employees[i].LastName = lineRead;
                                Console.WriteLine(employees[i].LastName);
                                lineCounter++;
                                break;
                            case 1:
                                employees[i].FirstName = lineRead;
                                lineCounter++;
                                break;
                            case 2:
                                employees[i].EmployeeType = lineRead;
                                lineCounter++;
                                break;
                            case 3:
                                if (employees[i].EmployeeType == "H")
                                {
                                    //Hourly hourlyEmployee = new Hourly(employees[i].FirstName, employees[i].LastName, employees[i].EmployeeType, Convert.ToDecimal(lineRead));
                                    Hourly hourlyEmployee = new Hourly(employees[i].FirstName, employees[i].LastName, Convert.ToDecimal(lineRead));
                                    //hourlyEmployee = (Hourly)employees[i];
                                    employees[i] = hourlyEmployee;
                                    //hourlyEmployee.HourlyRate = Convert.ToDouble(lineRead);
                                    Console.WriteLine(employees[i].LastName);
                                    //Console.WriteLine(employees[i].HourlyRate);
                                    Console.WriteLine(hourlyEmployee.LastName);
                                    Console.WriteLine(hourlyEmployee.HourlyRate);
                                }
                                else if (employees[i].EmployeeType == "S")
                                {
                                    Salaried salaryEmployee = new Salaried(employees[i].FirstName, employees[i].LastName, Convert.ToDecimal(lineRead));
                                    employees[i] = salaryEmployee;
                                    //salaryEmployee.Salary = Convert.ToDecimal(lineRead);
                                }
                                
                                lineCounter = 0;
                                i++;
                                break;
                        }
                        
                    }
                } //end using StreamReader
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (emptyFile)
                    Console.WriteLine("{fileName} is empty"); 
            }

            return employees;
        }  //end FileToArray method

        static void PrintEmployees(Employee[] employees)
        {
            foreach (Employee person in employees)
                Console.WriteLine(person);
        }  //end PrintEmployees method



        static void CreateEmployee(Employee[] employees)
        {
            int i = 0;
            int indexFound = -1;

            while (indexFound == -1 && i < employees.Length)
            {
                if (employees[i].FirstName == " ")
                    indexFound = i;
                i++;
            }    
             
            if (indexFound == -1)
                Console.WriteLine("There is no available space in the file");
            else
            {     
                Console.Write("Please enter the new employee's type. H, S, or O: ");
                string input = (Console.ReadLine()).ToUpper();
                if (input == "H")
                {
                    Hourly newHourly = new Hourly();
                    Console.Write("Enter the new employee's first name:");
                    newHourly.FirstName = Console.ReadLine();
                    Console.Write("Enter the new employee's last name:");
                    newHourly.LastName = Console.ReadLine();
                    Console.Write("Enter the new employee's hourly rate:");
                    newHourly.HourlyRate = Convert.ToDecimal(Console.ReadLine());
                    employees[indexFound] = newHourly;

                }
                else if (input == "S")
                {
                    Salaried newSalary = new Salaried();
                    Console.Write("Enter the new employee's first name:");
                    newSalary.FirstName = Console.ReadLine();
                    Console.Write("Enter the new employee's last name:");
                    newSalary.LastName = Console.ReadLine();
                    Console.Write("Enter the new employee's yearly salary:");
                    newSalary.Salary = Convert.ToDecimal(Console.ReadLine());
                    employees[indexFound] = newSalary;
                }
                else
                {
                    Console.Write("Enter the new employee's first name:");
                    employees[indexFound].FirstName = Console.ReadLine();
                    Console.Write("Enter the new employee's last name:");
                    employees[indexFound].LastName = Console.ReadLine();                 
                }
            }
        }  //end CreateEmployee method

        static void UpdateEmployee(Employee[] employees)
        {
            int indexFound = -1;
            bool inputValid = false;

            Console.Write("Please enter the first name of the employee to update:");
            string input_firstName = Console.ReadLine();
            Console.Write("Please enter the last name of the employee to update:");
            string input_lastName = Console.ReadLine();

            for (int i = 0; i < employees.Length; i++)
            {
                if (employees[i].FirstName == input_firstName && employees[i].LastName == input_lastName)
                {
                    indexFound = i;    
                }
            }

            //if a matching name is found, instantiate a blank Restaurant object
            if (indexFound != -1)
            {
                Console.Write("Enter one of the following to update: first name, last name, type, rate, salary: ");
                do
                {
                    
                    string input = (Console.ReadLine()).ToLower();
                    switch (input)
                    {
                        case "first name":
                            Console.Write("Enter new first name: ");
                            employees[indexFound].FirstName = Console.ReadLine();
                            inputValid = true;
                            break;
                        case "last name":
                            Console.Write("Enter new last name: ");
                            employees[indexFound].LastName = Console.ReadLine();
                            inputValid = true;
                            break;
                        case "type":
                            Console.Write("Enter new employee type: ");
                            employees[indexFound].EmployeeType = Console.ReadLine().ToUpper();
                            inputValid = true;
                            switch (employees[indexFound].EmployeeType)
                            {
                                case "H":
                                    Console.Write("Please enter the new employee's hourly rate: ");
                                    input = Console.ReadLine();
                                    Hourly newHourly = new Hourly(employees[indexFound].FirstName, employees[indexFound].LastName, Convert.ToDecimal(input));
                                    employees[indexFound] = newHourly;
                                    break;
                                case "S":
                                    Console.Write("Please enter the new employee's salary: ");
                                    input = Console.ReadLine();
                                    Salaried newSalaried = new Salaried(employees[indexFound].FirstName, employees[indexFound].LastName, Convert.ToDecimal(input));
                                    employees[indexFound] = newSalaried;
                                    break;
                                default:
                                    Employee newEmployee = new Employee(employees[indexFound].FirstName, employees[indexFound].LastName, employees[indexFound].EmployeeType);
                                    employees[indexFound] = newEmployee;
                                    break;
                            }
                            break;
                        case "rate":
                            inputValid = true;
                            if (employees[indexFound] is Hourly)
                            {
                                Console.Write("Please enter the new employee's hourly rate: ");
                                input = Console.ReadLine();
                                Hourly newEmployee = new Hourly(employees[indexFound].FirstName, employees[indexFound].LastName, Convert.ToDecimal(input));
                                employees[indexFound]= newEmployee;   
                            }
                            else
                                Console.WriteLine("This employee is not hourly.");
                            break;
                        case "salary":
                            inputValid = true;
                            if (employees[indexFound] is Salaried)
                            {
                                Console.Write("Please enter the new employee's salary: ");
                                input = Console.ReadLine();
                                Salaried newEmployee = new Salaried(employees[indexFound].FirstName, employees[indexFound].LastName, Convert.ToDecimal(input));
                                employees[indexFound]= newEmployee; 
                            }
                            else    
                                Console.WriteLine("This employee is not salaried.");
                            break;
                        default:
                            Console.Write("Invalid input.  Enter first name, last name, type, rate, or salary: ");
                            break;
                    }
                } while (!inputValid);
            }
            //if a matching name is not found, output a message
            else
                Console.WriteLine("Employee not found.");

        }  //end UpdateEmployee method
        static void DeleteEmployee(Employee[] employees)
        {
            int indexFound = -1;

            Console.Write("Please enter the first name of the employee to delete: ");
            string input_firstName = Console.ReadLine();
            Console.Write("Please enter the last name of the employee to delete: ");
            string input_lastName = Console.ReadLine();
 
            //loop through row of array and set restauntIndex if a matching name is found
            for (int i = 0; i < employees.Length; i++)
            {
                if (employees[i].FirstName == input_firstName && employees[i].LastName == input_lastName)
                {
                    indexFound = i;    
                }
            }

            //if a matching name is found, instantiate a blank Restaurant object
            if (indexFound != -1)
            {
                employees[indexFound] = new Employee();
                
            }
            //if a matching name is not found, output a message
            else
                Console.WriteLine("Employee not found.");
     
        } //end DeleteEmployee method

        static void ArrayToFile(Employee[] employees, string file)
        {
            try
            {  
                using (StreamWriter writer = new StreamWriter(file))
                {
                    for (int i = 0; i < employees.Length; i++)
                    {
                        writer.WriteLine(employees[i].LastName);
                        writer.WriteLine(employees[i].FirstName);
                        writer.WriteLine(employees[i].EmployeeType);
                        if (employees[i] is Hourly)
                        {  
                            //Hourly employee = new Hourly();
                           // employee = employees[i];
                            writer.WriteLine(((Hourly)employees[i]).HourlyRate);
                        }
                        else if (employees[i] is Salaried)
                        {   
                            //Salaried employee = new Salaried();    
                            writer.WriteLine(((Salaried)employees[i]).Salary);
                        }
                        else
                            writer.WriteLine(" ");
                    }     
                } //end using StreamWriter
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing to file.");
            }
        }  //end ArrayToFile method

    }  //end class
}  //end namespace