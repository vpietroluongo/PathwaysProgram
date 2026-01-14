using System;
using System.Globalization;
using System.Net;

namespace StudentGrades
{
    class Program
    {
        /*This program asks a user to input a number of students, the grade for those students, and then calculates their final grade.
            Prompt user for number of students
            Obtain number of students from user
            Validate student number
            For each student
                Prompt user for student's name
                Obtain student's name
                Repeat 5 times
                    Prompt user for homework grade
                    Obtain grade from user
                    Validate grade
                    Calculate homework average
                Repeat 3 times
                    Prompt user for quiz grade
                    Obtain grade from user
                    Validate grade
                    Calculate quiz average
                Repeat 2 times
                    Prompt user for exam grade
                    Obtain grade from user
                    Validate Grade
                    Calculate exam average
                Calculate weighted grade
                Convert number grade to letter grade
                Write out student's grade
        */
        static void Main(string[] args)
        {
            string input;
            int studentNum = 0;
            bool isValid = false;
            int grade;
            string name;
            int hwTotal = 0;
            int quizTotal = 0;
            int examTotal = 0;
            int hwAverage = 0;
            int quizAverage = 0;
            int examAverage = 0;
            int finalGradeNum = 0;
            string finalGradeLetter = "";

            //Prompt user for number of students and validate input
            Console.Write("Enter the number of students: ");
            while (isValid == false)
            {
                input = Console.ReadLine();
                if (int.TryParse(input, out studentNum))
                {
                    if (studentNum >= 1)
                        isValid = true;
                    else
                        Console.Write("Number must be greater than or equal to 1. Please retry: ");
                }  
                else
                {
                    Console.Write("Invalid input. Please enter a whole number: ");    
                }
            } //end while loop

            //For each student, get name and grades, calculate final grade, and write out final grade
            for (int i = 1; i <= studentNum; i++)
            {
                Console.Write("Enter student's name: ");
                name = Console.ReadLine();

                //get homework grades 
                for (int j = 1; j <= 5; j++)
                {
                    Console.Write($"Enter {name}\'s homework {j} grade: ");
                    input = Console.ReadLine();
                    grade = GradeValidity(input);
                    hwTotal += grade;
                    hwAverage = CalculateAverage(hwTotal, j);
                } //end homework for loop

                //get quiz grades 
                for (int j = 1; j <= 3; j++)
                {
                    Console.Write($"Enter {name}\'s quiz {j} grade: ");
                    input = Console.ReadLine();
                    grade = GradeValidity(input);
                    quizTotal += grade;
                    quizAverage = CalculateAverage(quizTotal, j);
                } //end quiz for loop */
               
                //get exam grades 
                for (int j = 1; j <= 2; j++)
                {
                    Console.Write($"Enter {name}\'s exam {j} grade: ");
                    input = Console.ReadLine();
                    grade = GradeValidity(input);
                    examTotal += grade;
                    examAverage = CalculateAverage(examTotal, j);
                } //end exam for loop */
                
                //Calculate final grade, convert to letter grade, and write to output
                finalGradeNum = FinalGrade(hwAverage, quizAverage, examAverage, 50, 30, 20);
                finalGradeLetter = FinalGrade(finalGradeNum);
                Console.WriteLine($"{name}\'s final grade is {finalGradeLetter}: {finalGradeNum}%");

                //clear variables for next student
                hwTotal = 0;
                quizTotal = 0;
                examTotal = 0;
            } //end for loop
        } //end Main method


        //This method checks that a valid number was entered and returns the grade
        static int GradeValidity(string input)
        {   
            bool IsValid = false;
            int grade;

            //Continue to prompt user for a grade until a valid number is input
            do
            {
                if (int.TryParse(input, out grade))
                {
                    if (grade >= 0 && grade <= 100)
                    {
                        IsValid = true;    
                    }   
                    else
                    {
                        Console.Write("Invalid input. Please enter whole number between 0 and 100: ");
                        input = Console.ReadLine();
                    } 
                }
                else
                {
                    Console.Write("Invalid type.  Please enter a whole number: ");
                    input = Console.ReadLine();
                } 
            } while (!IsValid);
            return grade;
        }

        //This method calculates the average
        static int CalculateAverage(int sum, int counter)
        {
            double average;
            int intAverage;

            average = sum / counter;   
            intAverage = Convert.ToInt32(Math.Round(average));
            return intAverage; 
        } //end CalculateAverage method

        //This method calculates the final number grade
        static int FinalGrade(int hwAverage, int quizAverage, int examAverage, double hwPercent, double quizPercent, double examPercent)
        {
            double weightedHW = 0;
            double weightedQuiz = 0;
            double weightedExam = 0;
            int weightedGrade = 0;
            
            weightedHW = hwAverage * (hwPercent / 100);  
            weightedQuiz = quizAverage * (quizPercent / 100);
            weightedExam = examAverage * (examPercent / 100);  

            weightedGrade = Convert.ToInt32(Math.Round(weightedHW + weightedQuiz + weightedExam));
            return weightedGrade;
        } //end FinalGrade method for number grade

        //This method determines the final letter grade
        static string FinalGrade(int finalGradeNum)
        {
            string letterGrade;

            if (finalGradeNum>= 90)
                letterGrade = "A";
            else if (finalGradeNum < 90 && finalGradeNum >= 80)
                letterGrade = "B";
            else if(finalGradeNum < 80 && finalGradeNum >= 70)   
                letterGrade = "C";
            else if(finalGradeNum < 70 && finalGradeNum >= 60)   
                letterGrade = "D";
            else if(finalGradeNum < 60)
                letterGrade = "F";
            else    
                letterGrade = "Something went wrong.";

            return letterGrade;

        } //end FinalGrade method for letter grade
    } //end class
} //end namespace