using System;

namespace MaximumNum
{
    class Program
    {
        static void Main(string[] args)
        {
           /* This program lets a user input as many number as they want until they enter the letter "z" and then finds the maximum of those numbers.

                    do
                        Prompt user for number.
                        Obtain number from user.
                        If input is an integer
                            If number is greater than the maximum
                               Set maximum to number
                        Else if the input is the flag
                                Exit loop
                        Else
                           Write out a type error message
                    while flag is not set

                    If at least one number has been entered
                        Write out the maximum number   
                    Else
                        Write out message that no numbers have been entered.                
           */ 

            //declare variables
            string input = "";              //holds value from user input
            int userNum;                    //holds the converted user input
            bool numFlag = false;           //numFlag will be used to determine if at least one number has been entered
            int maximum = int.MinValue;     //setting maximum to the smallest possible value of int

           do
           {
              //Prompt user for input
              Console.Write("Input a number. Enter the letter \"z\" when you are done inputting numbers: ");

              //Obtain input from the user
              input = Console.ReadLine();

              //Try to parse the input string to an integer
              if (int.TryParse(input, out userNum))
              {
                //set numFlag to true since at least one number has been received
                numFlag = true;

                //Check if the parsed integer is greater than the maximum
                if (userNum > maximum) 
                {
                   maximum = userNum;    
                }     
              } 
              //If the input string is not an integer but is the letter z, end the loop
              else if (input == "z")
              {
                Console.WriteLine("Done inputting numbers.");    
              } 
              //If the input string is not a valid value, write out an error message
              else 
              {
                 Console.WriteLine("Error: Invalid input.  Please enter a whole number.");   
              } 
           } while (input != "z");
           
           //Write out the maximum number if number(s) provided
           if (numFlag == true)
            {
                Console.WriteLine($"The maximum number is {maximum}");  
            }
           else
            {
              Console.WriteLine("You did not enter any numbers.");
            }
        } //end Main method
    } //end class
} //end namespace
