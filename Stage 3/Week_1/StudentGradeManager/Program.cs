using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace StudentGradeManager;

class Program
{
    static void Main(string[] args)
    {
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
                student.Average = CalculateAverage(student.TestScores);
                Console.Write($"|        {student.Average}");
                int count = Math.Abs(student.Average).ToString().Length;
                for (int i = count; i <= 6; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine($"|       {CalculateLetterGrade(student.Average)}      |");
            }
            Console.WriteLine($"The class average is {CalculateClassAverage(listOfStudents)}");
            int indexOfStudentWithMax = MaxAverage(listOfStudents);
            Console.WriteLine($"{listOfStudents[indexOfStudentWithMax].Name} has the highest average: {listOfStudents[indexOfStudentWithMax].Average}");
            Console.Write("Do you want to run again? (yes/no): ");
            input = (Console.ReadLine()).ToLower();
            bool validInput = false;
            while (validInput == false)
            {    
                if (input == "no")
                {
                   validInput = true; 
                    runAgain = false;
                }
                else if(input == "yes")
                {
                    validInput = true;
                }
                else
                {
                    Console.Write("Invalid input, enter yes or no: ");
                    input = Console.ReadLine();
                }
            }
        }

        
    }  

    static double CalculateAverage(List<int> scores)
    {
        double sum = 0;
        foreach (int score in scores)
        {
            sum += score;    
        }
        double average = Math.Round((sum / scores.Count), 1);
        return average;
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

    static string CalculateLetterGrade(double average)
    {
        string letterGrade = "";
        if (average >= 90 && average <= 100)
            letterGrade = "A";
        else if (average >= 80)
            letterGrade = "B";
        else if (average >= 70)
            letterGrade = "C";
        else if (average >= 60)
            letterGrade = "D";
        else if (average <=59 && average >= 0)
            letterGrade = "F";
        
        return letterGrade;
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
