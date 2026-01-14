using System;

namespace Power1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*This program repeatedly obtains a base and exponent from the user and prints out the value of the base taken to the exponent power until -1 is entered.
                ----Main Method
                        Do
                            Prompt user for base
                            Obtain input from user
                            Call Input Validity method passing input as an argument to obtain base
                            Check if -1 has been entered, and if not
                                Prompt user for exponent
                                Obtain input from user
                                Call Input Validity method passing input as argument to obtain exponent 
                                Check if -1 has been enter, and if not
                                    Call Power method passing base and exponent as arguments
                                    Write output
                        While input is not equal to -1

                ----Input Validity Method passes in the input and returns a number
                        do
                            If the input is a valid type
                                If input is greater than or equal to 1
                                    Set valid input flag to true
                                Else if the input is "-1"
                                    Set valid input flag to true
                                    Output quit message
                                Else if the input is < 1
                                    Output error message and promt user to re-enter number
                                    Obtain input from user
                                Else
                                    Output a type error message and prompt user to re-enter number
                                    Obtain input from user
                        While input flag is false
                        Return a number

                ----Power method passes in the base and exponent and returns the product
                        Set a new number equal to the base
                        For each number in the exponent, multiply the new number by the base
                        Return a number


            */
            
            //declare variables
            string input = "";
            int baseNum = 0;
            int exponent = 0;
            int product = 0;

            //Continue to prompt user for numbers until -1 is entered
            do
            {
                //Get base number from user
                Console.Write("Enter a base number or enter -1 to quit: ");
                input = Console.ReadLine();
                baseNum = InputValidity(input);

                //Continue if -1 to quit has not been entered
                if (baseNum != -1)
                {
                    //Get exponent number from user
                    Console.Write("Enter an exponent or enter -1 to quit: ");
                    input = Console.ReadLine();
                    exponent = InputValidity(input);

                    // Continue if -1 to quite has not bee entered
                    if (exponent != -1)
                    {
                        //Calculate and output power
                        product = Power(baseNum,exponent);
                        Console.WriteLine($"{baseNum} to the power of {exponent} is {product}");
                    }
                }
            } while (baseNum != -1 && exponent != -1);
        } //end Main method

        //This method checks the valdity of the user input
        static int InputValidity(string userInput)
        {
            //declare variables
            bool isValid = false;
            int number = 0;

            //Continue until a valid number is entered
            do
            { 
                 Console.WriteLine("We are here");
                //Check if the input is an integer
                if (int.TryParse(userInput, out number))
                {
                    //Check if the input is greater than or equal to 1
                    if (number >= 1)
                    {
                        isValid = true;
                    }  
                    //Check if the -1 to quit has been entered 
                    else if (number == -1)
                    {
                        isValid = true;
                        Console.WriteLine("Quitting program.");
                    }
                    //Check if the number if less than one and prompt user to enter new number
                    else
                    {
                        Console.Write("Number must be greater than or equal to 1.  Please try again: ");
                        userInput = Console.ReadLine();  
                    }    
                }
                //Output an error message and prompt user to enter new number
                else
                {
                    Console.Write("Invalid input.  Please try again: ");
                    userInput = Console.ReadLine();
                }
            } while (!isValid);

            return number;
        } //end InputValidity method

        //This method caluclates the base to the power of exponent
        static int Power(int baseNum, int exponent)
        {
            int newNum = baseNum;

            for (int i = 1; i < exponent; i++)
            {
                newNum *= baseNum;      
            }
            return newNum;
        } //end Power method
    } //end class
} //end namespace
