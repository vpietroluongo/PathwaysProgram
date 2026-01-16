using System;

namespace RestaurantRatings
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quitProgram = false;
            const int arraySize = 25;
            const int arrayColumns = 3;
            string[,] restaurants = new string[arraySize, arrayColumns];
            string fileName = "restaurants.txt";

            Console.WriteLine("Restaurant Ratings");
            do
            {  
                Console.WriteLine("Choose an option:");
                Console.WriteLine("L - Load list of restaurants, cuisines, and ratings");
                Console.WriteLine("S - Save list of restaurants, cuisine, and ratings");
                Console.WriteLine("C - Add a restaurant, cuisine, and rating");
                Console.WriteLine("R - Print list of all restaurants, cuisines, and ratings");
                Console.WriteLine("U - Update information for restaurant");
                Console.WriteLine("D - Delete a restaurant");
                Console.WriteLine("Q - Quit the program"); 

                string input = (Console.ReadLine()).ToUpper();

                switch (input)
                {
                    case "L":
                        Console.WriteLine("In L area");
                        restaurants = FileToArray(fileName, arraySize, arrayColumns);
                        break;
                    case "S":
                        Console.WriteLine("In S area");
                        break;
                    case "C":
                        Console.WriteLine("In C area");
                        break;
                    case "R":
                        Console.WriteLine("In R area");
                        break;
                    case "U":
                        Console.WriteLine("In U area");
                        break;
                    case "D":
                        Console.WriteLine("In D area");
                        break;
                    case "Q":
                        Console.WriteLine("In Q area");
                        quitProgram = true;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid option.");
                        break;
                } //end switch
            } while(!quitProgram);
        } //end Main method

        static string[,] FileToArray(string file, int arraySize, int arrayColumns)
        {
            string fileStatus = "";
            string[,] arrayFromFile = new string[arraySize, arrayColumns];

            try
            {
                //Open file and read it line by line until reach a null line.  
                using (StreamReader reader = File.OpenText(file))
                {
                    string lineRead = " ";
                    int i = 0;

                    while ((lineRead = reader.ReadLine()) != null)
                    {
                        //Parse the line read by comma, and put each parsed word into a new element for the same row.
                        string[] words = lineRead.Split(',');
                        for (int j = 0; j < words.Length; j++)
                        { 
                            arrayFromFile[i,j] = lineRead; 
                        }
                        i++;   
                    }    
                } //end using StreamReader
                fileStatus = "successful"; 
            }
            catch (Exception e)
            {
                fileStatus = "unsuccessful";
                Console.WriteLine($"File {file} not found.");    
            }
            finally
            {
                Console.WriteLine($"File load {fileStatus}. Returning to main menu.");    
            }
            
            return arrayFromFile; 
        } //end FileToArray method
    } //end class
} //end namespace
