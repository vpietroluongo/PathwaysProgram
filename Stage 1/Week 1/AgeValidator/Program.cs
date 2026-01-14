using System;
class AgeValidator
{
 /* do
      prompt user for age
      obtain age from user
      if age is an integer then
             if age is in a valid range (0-120)
                then print that it is
             else
                 provide a range error message
   
     else
             provide a type error error message 
while the age is a valid range or type   */

    static void Main()
    {
        int age;
        bool isValidAge = false;
        do
        {
            Console.Write("Please enter your age (0-120): ");
            string input = Console.ReadLine();
            // Try to parse the input string to an integer
            if (int.TryParse(input, out age))
            {
                // Check if the parsed integer is within the valid range (0 to 120)
                if (age >= 0 && age <= 120)
                {
                    isValidAge = true;
                    Console.WriteLine($"\nValid age entered: {age}");
                }
                else
                {
                    Console.WriteLine("Error: Age must be between 0 and 120. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid input. Please enter a numerical value.");
            }
        } while (!isValidAge);
        // The program continues here only after a valid age is entered
        Console.WriteLine("Program finished. Press any key to exit.");
        Console.ReadKey();
    }
}
