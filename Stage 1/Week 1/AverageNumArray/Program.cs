using System;
using System.Collections;
using System.Numerics;

namespace AverageNumArray
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Problem/requirements - for an array of numbers, find the average
                array is provided
                each element is an integer

            Algorithm - 
                I.	loop thru the array
                        for each element/row
                            increment the count
                            add the element to the total
                        end of for loop
                    
                II.		divide sum of elements by count of elements
                        average equals total divided by count (length of array for calculation)
                        
                III. 	display the average
            */

            int[] scores = {30, 40, 50, 60, 80, 76, 96, 92, 91, 100};
            //double average = 0.0;

            double average = CalcAverage(scores);

            //Console.WriteLine($"average = {total} \\ {scores.Length} = {average}");
            Console.WriteLine($"average = {average}");

        } //end Main method

        static double CalcAverage(int[] numbers)
        {
            double total = 0.0;
            double average = 0.0;

            for(int i = 0; i < scores.Length; i++)
            {
               total += scores[i];
            } //end for loop
        
        average = total / scores.Length;
        return average;
        } //end CalcAverage method
    } //end class
} //end namespace
