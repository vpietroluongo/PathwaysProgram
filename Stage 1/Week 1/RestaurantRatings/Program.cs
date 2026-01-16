using System;
using System.Globalization;
using System.Transactions;
using Microsoft.VisualBasic.FileIO;

namespace RestaurantRatings
{
    class Program
    {
        //This program reads a comma-delimitted file with information about up to 25 restaurant names, their type of cuisine, and star ratings, and lets the user modify and save that infos
        static void Main(string[] args)
        {
            bool quitProgram = false;
            const int arraySize = 25;
            const int arrayColumns = 3;
            string[,] restaurants = new string[arraySize, arrayColumns];
            string fileName = "restaurants.txt";

            Console.WriteLine("Restaurant Ratings");
            //loop until Q is entered and quitProgram flag changes
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
                        ArrayToFile(restaurants, fileName);
                        break;
                    case "C":
                        Console.WriteLine("In C area");
                        restaurants = AddRestaurant(restaurants);
                        break;
                    case "R":
                        Console.WriteLine("In R area");
                        PrintArray(restaurants);
                        break;
                    case "U":
                        Console.WriteLine("In U area");
                        restaurants = UpdateRestaurant(restaurants);
                        break;
                    case "D":
                        Console.WriteLine("In D area");
                        restaurants = DeleteRestaurant(restaurants);
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



        //This method reads a file line by line and returns the lines to a 2D array, where each row is the information for a different restaurant
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
                    //bool lineFound = false;
                    int i = 0;

                    while ((lineRead = reader.ReadLine()) != null)
                    {
                       // lineFound = true;
                        //Parse the line read by comma, and put each parsed word into a new element for the same row.
                        string[] words = lineRead.Split(',');
                        for (int j = 0; j < words.Length; j++)
                        { 
                            arrayFromFile[i,j] = words[j]; 
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



        //This method loops through the array and writes out each row on a new line
        static void PrintArray(string[,] lines)
        {
            int j;
            bool elementsFound = false;

            for (int i = 0; i < lines.GetLength(0); i ++)
            {
                for (j = 0; j < lines.GetLength(1); j++)
                {
                    if (lines[i,j] != "" && lines[i,j] != " " && lines[i,j] != null)
                    { 
                        Console.Write($"{lines[i,j]}  ");
                 //       elementsFound = true;
                    } //end if
                }  //end for loop for j 
                Console.WriteLine("");   
            }  //end for loop for i 

        //    if (elementsFound = false)
         //       Console.WriteLine("Empty list of restaurants");
        } //end PrintArray method



        /*This method loops through one index of a 2D array looking for an available space.  
          If space is not available 
            Output an error message
          else
            update the first available spot with restaurant name, cuisine, and rating
          Return a 2D array
        */
        static string[,] AddRestaurant(string[,] lines)
        {
            int indexFound = -1;
            int i = 0;
            int j = 0;
            int input;
            bool validInput = false;

            //loop through row of array with the assumption that if the restaurant element is empty, the other values in that row are empty as well
            for (i = 0; i < lines.GetLength(0); i++)
            {
                    //only update indexFound value if it has not already been changed so that we force the first available space to get updated later
                    if (lines[i,j] == " " && indexFound == -1)
                    {
                        indexFound = i;
                    }   
            }

            //if no available space was found, write out a message
            if (indexFound == -1)
            {
                Console.WriteLine("No space available. Returning to main menu.");
            }
            //if available space was found, update that space with user input
            else
            {
                Console.Write("Enter a restaurant name:");
                lines[indexFound,0] = Console.ReadLine();
                Console.Write("Enter the type of cuisine:");
                lines[indexFound,1] = Console.ReadLine();
                Console.Write("Enter a rating:");
                do
                {
                    input = Convert.ToInt32(Console.ReadLine());
                    if (input >= 0 && input <= 5)
                    {   
                        validInput = true;  

                        lines[indexFound,2] = Convert.ToString(input);  
                    }
                    else 
                        Console.Write("Star rating must be between 0 and 5.  Please re-enter a rating:");        
                } while (!validInput);
            }

            return lines;
        } //end AddRestaurant method



        /*This method prompts the user for a restaurant name and then loops through the rows of a 2D array looking for a match.
          If a match is found
            prompt user for what they want to update
            do 
                obtain category from user
                if name
                    update name element
                if cuisine
                    update cuisine element
                if rating
                    check valid range
                        update rating element
                if other
                    Write out error message
            while category flag set to false
          else
            Write out an error message
          Return a 2D array
        */
        static string[,] UpdateRestaurant(string[,] lines)
        {
            string name;
            int j = 0;
            int restaurantIndex = -1;

            Console.Write("Please enter the restaurant to update:");
            name = Console.ReadLine();
 
            //loop through row of array and set restauntIndex if a matching name is found
            for (int i = 0; i < lines.GetLength(0); i++)
            {
                if (lines[i,j] == name)
                {
                    restaurantIndex = i;    
                }
            }

            //if a match is found, prompt user for what they want to update and update corresponding element
            if (restaurantIndex != -1)
            {
                bool inputValid = false;
                do
                {
                    Console.WriteLine("Enter one of the following to update: name, cuisine, rating:");
                    string input = (Console.ReadLine()).ToLower();
                    switch (input)
                    {
                        case "name":
                            Console.Write("Enter new name:");
                            lines[restaurantIndex,0] = Console.ReadLine();
                            inputValid = true;
                            break;
                        case "cuisine":
                            Console.Write("Enter new cuisine:");
                            lines[restaurantIndex,1] = Console.ReadLine();
                            inputValid = true;
                            break;
                        case "rating":
                            Console.Write("Enter new rating:");
                            bool validNum = false;
                            do
                            {
                                int newRating = Convert.ToInt32(Console.ReadLine());
                                if (newRating >= 0 && newRating <= 5)
                                {
                                    validNum = true;
                                    lines[restaurantIndex,2] = Convert.ToString(newRating);
                                }
                                else
                                    Console.Write("Please enter a number between 0 and 5:");
                            }
                            while (!validNum);
                            inputValid = true;
                            break;
                        default:
                            Console.Write("Invalid input.  Enter name, cuisine, or rating:");
                            break;
                    }   //end switch 
                } while (!inputValid); 
            } //end if restaurant found
            //if a match is nout found, output a message
            else
                Console.WriteLine("Restaurant is not found.");

            return lines;
        }  //end UpdateRestaurant method       



        /*This method promts the user for a restaurant name and then loops through the rows of a 2D array looking for a match.
          If a match is found
            delete the elements in the array associated with that restaurant
          else
            Write out an error message
          Return a 2D array
        */
        static string[,] DeleteRestaurant(string[,] lines)
        {
           string name;
            int j = 0;
            int restaurantIndex = -1;

            Console.Write("Please enter the restaurant to delete:");
            name = Console.ReadLine();
 
            //loop through row of array and set restauntIndex if a matching name is found
            for (int i = 0; i < lines.GetLength(0); i++)
            {
                if (lines[i,j] == name)
                {
                    restaurantIndex = i;    
                }
            }

            //if a matching name is found, loop through the columns to blank out each element
            if (restaurantIndex != -1)
            {
                for (j = 0; j < lines.GetLength(1); j++)
                {
                    lines[restaurantIndex,j] = " ";
                }
            }
            //if a matching name is not found, output a message
            else
                Console.WriteLine("Restaurant not found.");

            return lines;      
        } //end DeleteRestaurant method


        /*This method takes in a 2D array and a file, loops through the array to create comma delimtted lines for each row, and writes each line to a file
        */
        static void ArrayToFile(string[,] restaurants, string file)
        {   
            string writeStatus = "";
            try
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    for (int i = 0; i < restaurants.GetLength(0); i++)
                    {
                        string newString = "";
                        for (int j = 0; j < restaurants.GetLength(1); j++)
                        {
                            //if at the the end of the row, don't include a comma at the end of the new string
                            if (j == (restaurants.GetLength(1) - 1))
                            {  
                                //if the element is blank, don't include any commas
                                if (restaurants[i,j] == " " || restaurants[i,j] == "" || restaurants[i,j] == null)
                                {
                                    newString = " ";
                                }
                                else
                                {
                                    newString = newString + restaurants[i,j];
                                }
                            }
                            //if not at the end of the row, include a comma at the end of the new string 
                            else
                            {
                                //if the element is blank, don't include any commas
                                if (restaurants[i,j] == " " || restaurants[i,j] == "" || restaurants[i,j] == null)
                                {     
                                    newString = " ";
                                }
                                else
                                {
                                    newString = newString + restaurants[i,j] + ",";
                                }
                            }
                        } //end for loop for j

                        writer.WriteLine(newString);
                    } // end for loop for i
                    writeStatus = "Successful";
                } //end using StreamWriter
            } //end try
            catch (Exception e)
            {
                writeStatus = "Failed";
                Console.WriteLine("Error writing to file.");   
            } //end catch
            finally
            {
                Console.WriteLine($"{writeStatus} write to file.");   
            } //end finally
        } //end ArrayToFile method
    } //end class
} //end namespace
