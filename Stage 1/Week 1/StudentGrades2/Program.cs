using System;
using System.Data;

namespace StudentGrades2
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
            string name;
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

                //get 5 homework grades, 3 quiz grades, 2 exam grades and return the average of each 
                hwAverage = GetGrades(name, "homework", 5);
                quizAverage = GetGrades(name, "quiz", 3);
                examAverage = GetGrades(name, "exam", 2);
                
                //Calculate final grade, convert to letter grade, and write to output
                finalGradeNum = FinalGrade(hwAverage, quizAverage, examAverage, 50, 30, 20);
                finalGradeLetter = FinalGrade(finalGradeNum);
                Console.WriteLine($"{name}\'s homework average is {hwAverage}.");
                Console.WriteLine($"{name}\'s quiz average is {quizAverage}.");
                Console.WriteLine($"{name}\'s exam average is {examAverage}.");
                Console.WriteLine($"{name}\'s final grade is {finalGradeLetter}: {finalGradeNum}%.");

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
            /* bool test = false;
            switch (finalGradeNum)
            {
                case  >= 90:
                    Console.WriteLine("This is true");
                    break;
                case >= 80:
                    Console.WriteLine("This is false");
                    break;
            }
            */

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

        /* For a certain number of times passed in the counter, this method gets a grade from user, 
        calls a method to validate it is valid, calls a method to calculate the average, and then returns the average */
        static int GetGrades(string name, string gradeType, int counter)
        {
            string input;
            int grade = 0;
            int gradeTotal = 0;
            int gradeAverage = 0;

	        for (int j = 1; j <= counter; j++)
            {
                Console.Write($"Enter {name}\'s {gradeType} {j} grade: ");
                input = Console.ReadLine();
                grade = GradeValidity(input);
                gradeTotal += grade;           
            }

            gradeAverage = CalculateAverage(gradeTotal, counter); 
            return gradeAverage;
        } //end GetGrades method
    } //end class
} //end namespace
