using System;

namespace Power2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*This program obtains a base from the user, repeatedly obtains an exponent from the user, each one greater than the last, 
              and prints out the value of the base taken to the exponent power until -1 is entered.
                ----Main Method
                        Prompt user for base
                        Obtain input from user
                        Call Input Validity method passing input as an argument to obtain base
                        While input is not equal to -1
                            Prompt user for exponent
                            Obtain input from user
                            Call Input Validity method passing input as argument to obtain exponent 
                            Check if -1 has been enter, and if not
                                Call Power method passing base and exponent as arguments
                                Write output
                        

                ----Input Validity Method for the base will pass in the input and return a number 
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


                ----Input Validity Method for the exponent will have the same logic as the base version but an additional check for if the input
                    is less than or equal to the previous exponent 


                ----Power method passes in the base and expoent and returns the product
                        Set a new number equal to the base
                        For each number in the exponent, multiply the new number by the base
                        Return a number


            */
            
            //declare variables
            string input = "";
            int baseNum = 0;
            int exponentStart = 0;
            int exponentEnd = 0;
            int product = 0;
            int prevExponent = 0;
            string stepFlag = "base";


            //Continue to prompt user for exponent until -1 is entered
            do
            {
                switch (stepFlag)
                {
                    case "base":
                        //Get base number from user
                        Console.Write("Enter a base number or enter -1 to quit: ");
                        input = Console.ReadLine();
                        baseNum = InputValidity(input);
                        stepFlag = "exponent start";
                        break;
                    case "exponent start":
                        //Get start exponent number from user
                        Console.Write("Enter a starting exponent or enter -1 to quit: ");
                        input = Console.ReadLine();
                        exponentStart = InputValidity(input);
                        stepFlag = "exponent end";
                        break;
                    case "exponent end":
                        //Get end exponent number from user
                        Console.Write("Enter an ending exponent or enter -1 to quit: ");
                        input = Console.ReadLine();
                        exponentEnd = InputValidity(input,exponentStart);
                        stepFlag = "done";
                        break;
                    default:
                        Console.WriteLine("Something went wrong.");
                        break;
                }

                // Continue if -1 to quit has not been entered
                if ((baseNum != -1 || exponentStart != -1  || exponentEnd != -1) && stepFlag == "done")                     
                {
                    for (int i = exponentStart; i <= exponentEnd; i++)
                    {
                        //Calculate and output power
                        product = Power(baseNum,i);
                        Console.WriteLine($"{baseNum} to the power of {i} is {product}");
                        //reset prevExponent to zero every time enter loop
                        prevExponent = 0;
                        stepFlag = "base";
                    }
                }
            } while (baseNum != -1 && exponentStart != -1 && exponentEnd != -1);
            Console.WriteLine("Quitting program.");
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
                    }
                    //Check if the number if less than one and prompt user to enter new number
                    else if (number < 1)
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
        } //end InputValidity method for base

        //This method checks the valdity of the user input
        static int InputValidity(string userInput, int prevExponent)
        {
            //declare variables
            bool isValid = false;
            int number = 0;

            //Continue until a valid number is entered
            do
            {  
                //Check if the input is an integer
                if (int.TryParse(userInput, out number))
                {
                    //Check if the input is greater than or equal to 1
                    if (number >= 1  && number > prevExponent)
                    {
                        isValid = true;
                    }  
                    //Check if the -1 to quit has been entered 
                    else if (number == -1)
                    {
                        isValid = true;
                    }
                    //Check if the current exponent is greater than the previously used exponent, and prompt user to enter new number
                    else if (number <= prevExponent)
                    {
                        Console.Write("Invalid number.  Enter a number that is greater than the starting exponent: ");
                        userInput=Console.ReadLine();
                    }
                    //Check if the number if less than one, and prompt user to enter new number
                    else if (number < 1)
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
        } //end InputValidity method for exponent

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
