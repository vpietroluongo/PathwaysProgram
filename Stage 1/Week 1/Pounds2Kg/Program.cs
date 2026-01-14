using System;

namespace Pounds2Kg
{
 class Program
 {
    static void Main(string[] args)
    {
      /*This program prompts a user for a weight in pounds, converts that number to kilograms, and then prints the weight.
        1. Prompt user for weight in pounds
        2. Obtain the weight in pounds from user
        3. Convert the weight in pounds to weight in kilograms
        4. Output the weight in kilograms
      */

      //Step 1: Prompt user for weight in pounds
      Console.Write("Enter weight in pounds:");

      //Step 2: Read input from user
      double userPounds = Convert.ToDouble(Console.ReadLine());

      //Step 3:  Call method to convert pounds to kilograms
      double kilograms = Pounds2Kg(userPounds);

      //Step 4: Write output to console
      Console.WriteLine($"Print Line 1: {userPounds} pounds is {kilograms} kilograms"); 
      Console.WriteLine("Print line 2: " + userPounds + " pounds is " + kilograms + " kilograms");   
      Console.WriteLine("Print Line 3: {0} pounds is {1} kilograms", userPounds, kilograms);  
    } //end Main method 

    static double Pounds2Kg(double pounds)
    {
      return pounds * 0.453;       
    } //end Pounds2Kg method  
 } //end class
} //end namespace
