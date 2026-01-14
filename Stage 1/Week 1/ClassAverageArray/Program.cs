using System;

namespace ClassAverageArray
{
    class Program
    {
        static void Main(string[] args)
        {
            /*This program will take an array of students' scores and determine each student's average, class average, class maximum, 
            and class minimum.

            Declare array of scores for multiple students where each column is a different student
            Call method to calculate each student's average
                For each student 
                    Calculate average
                Return array of students' averages
            Call method to calculate class average
                For each student 
                    Add average to the total
                Calculate average
                Return class average
            Call method to determine maximum of student's averages
                For each student
                    If average is maximum
                        set maximum to their average
            Call method to determine minimum of student's averages  
                 For each student
                    If average is minimum
                        set minimum to their average  
            For each student
                Write out student average
            Write out class average
            Write out class maximum average
            Write out class minimum average
            */

            int[,] scores = { { 90, 76, 56, 88 }, { 90, 82, 64, 62 }, { 90, 98, 0, 98 } };

            double [] studentAverages = CalcAverages(scores); 
            double classAverage = CalcAverage(studentAverages);
            double maxAverage = MaxNum(studentAverages);
            double minAverage = MinNum(studentAverages);
            
            //For each student, write out their average
            for (int i = 0; i < studentAverages.Length; i++)
            {
                Console.WriteLine($"Student {i} average is {studentAverages[i]}");
            }
            Console.WriteLine($"Class average is {classAverage}.");
            Console.WriteLine($"Maximum average is {maxAverage}.");
            Console.WriteLine($"Minimum average is {minAverage}.");

        } //end Main method

        //This method takes in a 2D array of numbers and returns an array of averages for each column
        static double[] CalcAverages(int[,] numbers)
        {
            //declare an array for the number of students
            double[] averages = new double[numbers.GetLength(1)];

            //for each student (column) sum their exam scores (row) and then calculate their average
            for (int colIndex = 0; colIndex < numbers.GetLength(1); colIndex++)
            {
                double total = 0.0;
                for (int rowIndex = 0; rowIndex < numbers.GetLength(0); rowIndex++)
                {
                    total += numbers[rowIndex, colIndex];
                }
                averages[colIndex] = total / numbers.GetLength(0);
            }

            return averages;
        }

        //This method takes in a 1 dimensional array of numbers and returns the average of those number
        static double CalcAverage(double[] numbers)
        {
            double total = 0.0;

            foreach (double num in numbers)
            {
                total += num;
            }

            double average = total / numbers.Length;
            return average;
        }

        //This method takes in a 1 dimensional array of numbers and returns the maximum of those numbers
        static double MaxNum(double[] numbers)
        {
            double maximum = 0.0;

            foreach (double num in numbers)
            {
                if (num > maximum)
                    maximum = num;
            }

            return maximum;
        } //end MaxNum method

        //This method takes in a 1 dimensional array of numbers and returns the minimum of those numbers
        static double MinNum(double[] numbers)
        {
            double minimum = 9999.9;

            foreach (double num in numbers)
            {
                if (num < minimum)
                    minimum = num;
            }

            return minimum;
        }
    } //end class

} //end namespace
