using System;
using System.Net.NetworkInformation;

namespace EmployeeBonus
{
    class Program
    { 
        //This program reads a file with information about up to 25 employees: last name, first name, employee type, and salary/hourly rate, 
        //and lets the user modify and save that info
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


        //This method reads a file line by line, where every 4 lines is associated with a different employee, and returns the lines to a 1D array of Employee objects, 
        // where each object has the information for a different employee
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
                        switch (lineCounter)
                        {
                            case 0:
                                employees[i] = new Employee();
                                employees[i].LastName = lineRead;
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
                                //Check if the employee type is Hourly
                                if (employees[i].EmployeeType == "H")
                                {
                                    Hourly hourlyEmployee = new Hourly(employees[i].FirstName, employees[i].LastName, Convert.ToDecimal(lineRead));
                                    employees[i] = hourlyEmployee;
                                }
                                //Check if the employee type is Salaried
                                else if (employees[i].EmployeeType == "S")
                                {
                                    Salaried salaryEmployee = new Salaried(employees[i].FirstName, employees[i].LastName, Convert.ToDecimal(lineRead));
                                    employees[i] = salaryEmployee;
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



        //For each Employee object in the array, write out the information for that employee
        static void PrintEmployees(Employee[] employees)
        {
            foreach (Employee person in employees)
                Console.WriteLine(person);
        }  //end PrintEmployees method


        //This method looks for an available space, and if it finds one, adds an employee to the array of Employee objects
        static void CreateEmployee(Employee[] employees)
        {
            int i = 0;
            int indexFound = -1;
            bool validInput = false;

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
                 do
                 {   
                    string input = (Console.ReadLine()).ToUpper();
                    if (input == "H")
                    {
                        validInput = true;
                        Hourly newHourly = new Hourly();
                        Console.Write("Enter the new employee's first name: ");
                        newHourly.FirstName = Console.ReadLine();
                        Console.Write("Enter the new employee's last name: ");
                        newHourly.LastName = Console.ReadLine();
                        Console.Write("Enter the new employee's hourly rate: ");
                        newHourly.HourlyRate = Convert.ToDecimal(Console.ReadLine());
                        employees[indexFound] = newHourly;

                    }
                    else if (input == "S")
                    {
                        validInput = true;
                        Salaried newSalary = new Salaried();
                        Console.Write("Enter the new employee's first name: ");
                        newSalary.FirstName = Console.ReadLine();
                        Console.Write("Enter the new employee's last name: ");
                        newSalary.LastName = Console.ReadLine();
                        Console.Write("Enter the new employee's yearly salary: ");
                        newSalary.Salary = Convert.ToDecimal(Console.ReadLine());
                        employees[indexFound] = newSalary;
                    }
                    else if (input == "O")
                    {
                        validInput = true;
                        employees[indexFound].EmployeeType = input; 
                        Console.Write("Enter the new employee's first name: ");
                        employees[indexFound].FirstName = Console.ReadLine();
                        Console.Write("Enter the new employee's last name: ");
                        employees[indexFound].LastName = Console.ReadLine();                   
                    }
                    else 
                    {
                        Console.WriteLine("Please enter H, S , or O: ");                 
                    }
                 } while (!validInput);
            }
        }  //end CreateEmployee method

        //This method updates the information for an employee in the Employee array if it finds a matching first and last name
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

            //if a matching name is found, update the specified info
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
                            Console.Write("Enter new employee type: H, S, or O: ");
                            do
                            {
                                employees[indexFound].EmployeeType = Console.ReadLine().ToUpper();
                                switch (employees[indexFound].EmployeeType)
                                {
                                    case "H":
                                        inputValid = true;
                                        Console.Write("Please enter the new employee's hourly rate: ");
                                        input = Console.ReadLine();
                                        Hourly newHourly = new Hourly(employees[indexFound].FirstName, employees[indexFound].LastName, Convert.ToDecimal(input));
                                        employees[indexFound] = newHourly;
                                        break;
                                    case "S":
                                        inputValid = true;
                                        Console.Write("Please enter the new employee's salary: ");
                                        input = Console.ReadLine();
                                        Salaried newSalaried = new Salaried(employees[indexFound].FirstName, employees[indexFound].LastName, Convert.ToDecimal(input));
                                        employees[indexFound] = newSalaried;
                                        break;
                                    case "O":
                                        inputValid = true;
                                        Employee newEmployee = new Employee(employees[indexFound].FirstName, employees[indexFound].LastName);
                                        employees[indexFound] = newEmployee;
                                        break;
                                    default:
                                        Console.Write("Please enter H, S, or O: ");
                                        break;
                                }
                            } while (!inputValid);
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


        //This method overwrites an element with a blank Employee object if a matching first and last name is found
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


        //This method writes to a file each proprty associated with an employee on a separate line
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