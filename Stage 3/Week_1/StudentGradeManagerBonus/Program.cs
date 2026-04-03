using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace StudentGradeManager;

class Program
{
    static void Main(string[] args)
    {
        string filename = "StudentReport.txt";
        if (File.Exists(filename))
        {
            Console.Write("Do you want to see the last saved report? (yes/no): ");
            string yesno = (Console.ReadLine()).ToLower();
            if (yesno == "yes")
            {
                try
                {
                    string lineText = "";
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        while ((lineText = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(lineText);
                        }   
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error reading file. Error message: {e.Message}");
                }
                finally
                {
                    Console.WriteLine("Read file processing complete");
                }
            }
        }
        else
        {
            Console.WriteLine("File does not exist.");
        }
        
        bool runAgain = true;
        while (runAgain)
        {
            Console.Write("How many students do you want to enter? ");
            string input = Console.ReadLine();
            int numberOfStudents;
            while (!int.TryParse(input, out numberOfStudents)  || numberOfStudents <=0)
            {
                Console.Write("Invalid input.  Please enter a whole number greater than zero: ");
                input = Console.ReadLine();
            }
            
            List<Student> listOfStudents = new List<Student>();
            for (int i = 0; i < numberOfStudents; i++)
            {
                Console.Write($"Enter student {i + 1}'s name: ");
                string name = Console.ReadLine();
                List<int> testScores = new List<int>();
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($"Enter student {i + 1}'s test score #{j + 1}: ");
                    input = Console.ReadLine();
                    int score;
                    while (!int.TryParse(input, out score) || score < 0 || score > 100)
                    {
                        Console.Write("Invalid inpud.  Enter integer between 0 and 100: ");
                        input = Console.ReadLine();
                    }
                    testScores.Add(score);
                }
                Student newStudent = new Student(name, testScores);
                listOfStudents.Add(newStudent);

            }

            Console.WriteLine("| Name               | Average Score | Letter Grade |");
            foreach (Student student in listOfStudents)
            {
                Console.Write($"| {student.Name}");
                for (int i = student.Name.Length; i <= 18; i++)
                {
                    Console.Write(" ");
                }
                student.CalculateAverageAndGrade();
                Console.Write($"|        {student.Average}");
                int count = Math.Abs(student.Average).ToString().Length;
                for (int i = count; i <= 6; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine($"|       {student.LetterGrade}      |");
            }
            Console.WriteLine($"The class average is {CalculateClassAverage(listOfStudents)}");
            int indexOfStudentWithMax = MaxAverage(listOfStudents);
            Console.WriteLine($"{listOfStudents[indexOfStudentWithMax].Name} has the highest average: {listOfStudents[indexOfStudentWithMax].Average}");
            
            Console. Write("Do you want to save the report? (yes/no): ");
            input = (Console.ReadLine()).ToLower();
            while (input != "yes" && input != "no")
            {
                Console.Write("Invalid input, enter yes or no: ");
                input = Console.ReadLine();
            }
            if (input == "yes")
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    try
                    {
                        foreach (Student student in listOfStudents)
                        {
                            sw.WriteLine($"Name: {student.Name} | Average: {student.Average} | Letter grade: {student.LetterGrade}");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error writing to file.  Error message: e.Message");
                    }
                    finally
                    {
                        Console.WriteLine("File processing complete");
                    }
                }

            }


            Console.Write("Do you want to run again? (yes/no): ");
            input = (Console.ReadLine()).ToLower();
            if (input == "no")
                runAgain = false;
            else if (input != "yes")
            {
                Console.Write("Invalid input, enter yes or no: ");
                input = Console.ReadLine();
            }
        }

        
    }  


    static double CalculateClassAverage(List<Student> students)
    {
        double sum = 0;
        foreach (Student student in students)
        {
            sum += student.Average;
        }

        double average = Math.Round((sum / students.Count), 1);
        return average;
    }

    static int MaxAverage(List<Student> students)
    {
        double max = double.MinValue;
        int indexOfStudentWithMax = int.MinValue;
        for (int i = 0; i < students.Count; i ++)
        {
            if (students[i].Average > max)
            { 
                max = students[i].Average;
                indexOfStudentWithMax = i;
            }
        }

        return indexOfStudentWithMax;
    }

}
