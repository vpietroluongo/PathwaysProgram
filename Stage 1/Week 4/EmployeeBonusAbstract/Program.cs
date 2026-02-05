using System;
using System.Net.NetworkInformation;

namespace EmployeeBonusAbstract
{
    class Program
    { 
        //This program reads a file with information about employees: last name, first name, employee type, and salary/hourly rate, 
        //and lets the user modify and save that info
        static void Main(string[] args)
        {
            bool quitProgram = false;
            List<Employee> employees = new List<Employee>();
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
                        employees = FileToList(fileName);
                        break;
                    case "S":
                        Console.WriteLine("In S area");
                        ListToFile(employees, fileName);
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


        //This method reads a file line by line, where every 4 lines is associated with a different employee, and returns the lines in a list of Employee objects, 
        // where each object has the information for a different employee
         static List<Employee> FileToList(string file)
        {
            bool emptyFile = true;
            string lineRead = "";
            //int i = 0;
            int lineCounter = 0;
            string newFirstName = "";
            string newLastName = "";
            string newEmployeeType = "";
            List<Employee> employees = new List<Employee>();

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
                                newLastName = lineRead;
                                lineCounter++;
                                break;
                            case 1:
                                newFirstName = lineRead;
                                lineCounter++;
                                break;
                            case 2:
                                newEmployeeType = lineRead;
                                lineCounter++;
                                break;
                            case 3:
                                //Check if the employee type is Hourly
                                if (newEmployeeType == "H")
                                {
                                    employees.Add(new Hourly(newFirstName, newLastName, Convert.ToDecimal(lineRead)));
                                }
                                //Check if the employee type is Salaried
                                else if (newEmployeeType == "S")
                                {                                  
                                    employees.Add(new Salaried(newFirstName, newLastName, Convert.ToDecimal(lineRead)));
                                }
                                
                                lineCounter = 0;
                                //i++;
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



        //For each Employee object in the list, write out the information for that employee
        static void PrintEmployees(List<Employee> employees)
        {
            foreach (Employee person in employees)
                Console.WriteLine(person);
            Console.WriteLine(employees.Count);
        }  //end PrintEmployees method


        //This method adds an employee objec to the end of the list
        static void CreateEmployee(List<Employee> employees)
        {
            bool validInput = false;
  
            Console.Write("Please enter the new employee's type. H or S: ");
                do
                {   
                    string input = (Console.ReadLine()).ToUpper();
                    if (input == "H")
                    {
                        validInput = true;
                        Console.Write("Enter the new employee's first name: ");
                        string newFirstName = Console.ReadLine();
                        Console.Write("Enter the new employee's last name: ");
                        string newLastName = Console.ReadLine();
                        Console.Write("Enter the new employee's hourly rate: ");
                        decimal newHourlyRate = Convert.ToDecimal(Console.ReadLine());
                        employees.Add(new Hourly(newFirstName, newLastName, newHourlyRate));
                    }
                    else if (input == "S")
                    {
                        validInput = true;
                        Console.Write("Enter the new employee's first name: ");
                        string newFirstName = Console.ReadLine();
                        Console.Write("Enter the new employee's last name: ");
                        string newLastName = Console.ReadLine();
                        Console.Write("Enter the new employee's yearly salary: ");
                        decimal newSalary = Convert.ToDecimal(Console.ReadLine());
                        employees.Add(new Salaried(newFirstName, newLastName, newSalary));
                    }
                    // else if (input == "O")
                    // {
                    //     validInput = true;
                    //     Console.Write("Enter the new employee's first name: ");
                    //     string newFirstName = Console.ReadLine();
                    //     Console.Write("Enter the new employee's last name: ");
                    //     string newLastName = Console.ReadLine();  
                    //     employees.Add(new Employee(newFirstName, newLastName));                 
                    // }
                    else 
                    {
                        Console.Write("Please enter H or S: ");                 
                    }
                 } while (!validInput);
        }  //end CreateEmployee method



        //This method updates the information for an employee in the list of Employee objects if it finds a matching first and last name
        static void UpdateEmployee(List<Employee> employees)
        {
            bool inputValid = false;

            Console.Write("Please enter the first name of the employee to update:");
            string input_firstName = Console.ReadLine();
            Console.Write("Please enter the last name of the employee to update:");
            string input_lastName = Console.ReadLine();

            //starting at element 0, iteratate through each employee (e) and return the index of the first one where the first and last name are equal to the input
            int indexFound = employees.FindIndex(0, e => e.FirstName == input_firstName && e.LastName == input_lastName);    


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
                            Console.Write("Enter new employee type: H or S: ");
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
                                    // case "O":
                                    //     inputValid = true;
                                    //     Employee newEmployee = new Employee(employees[indexFound].FirstName, employees[indexFound].LastName);
                                    //     employees[indexFound] = newEmployee;
                                    //     break;
                                    default:
                                        Console.Write("Please enter H or S: ");
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


        //This method removes an element from the list if a matching first and last name are found
        static void DeleteEmployee(List<Employee> employees)
        {
            Console.Write("Please enter the first name of the employee to delete: ");
            string input_firstName = Console.ReadLine();
            Console.Write("Please enter the last name of the employee to delete: ");
            string input_lastName = Console.ReadLine();
 
            //starting at element 0, iteratate through each employee (e) and return the index of the first one where the first and last name are equal to the input
            int indexFound = employees.FindIndex(0, e => e.FirstName == input_firstName && e.LastName == input_lastName);  

            //if a matching name is found, remove the element at that index
            if (indexFound != -1)
            {
                employees.RemoveAt(indexFound);
                //employees.Remove(e => e.FirstName == input_firstName && e.LastName == input_lastName);
                
            }
            //if a matching name is not found, output a message
            else
                Console.WriteLine("Employee not found.");
     
        } //end DeleteEmployee method


        //This method writes to a file each property associated with an employee on a separate line
        static void ListToFile(List<Employee> employees, string file)
        {
            try
            {  
                using (StreamWriter writer = new StreamWriter(file))
                {
                    for (int i = 0; i < employees.Count; i++)
                    {
                        writer.WriteLine(employees[i].LastName);
                        writer.WriteLine(employees[i].FirstName);
                        writer.WriteLine(employees[i].EmployeeType);
                        if (employees[i] is Hourly)
                        {  
                            writer.WriteLine(((Hourly)employees[i]).HourlyRate);
                        }
                        else if (employees[i] is Salaried)
                        {     
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