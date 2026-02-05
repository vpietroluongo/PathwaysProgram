using System;
using System.Globalization;
using System.Transactions;
using Microsoft.VisualBasic.FileIO;

namespace RestaurantObjects
{
    class Program
    {
        //This program reads a comma-delimitted file with information about up to 25 restaurant names, their type of cuisine, and star ratings, and lets the user modify and save that info
        static void Main(string[] args)
        {
            bool quitProgram = false;
            const int arraySize = 25;
            Restaurant[] restaurants = new Restaurant[arraySize];
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
                        restaurants = FileToArray(fileName, arraySize);
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



        //This method reads a file line by line and returns the lines to a 1D array of Restaurant objects, where each object has the information for a different restaurant
        static Restaurant[] FileToArray(string file, int arraySize)
        {
            string fileStatus = "";
            Restaurant[] arrayFromFile = new Restaurant[arraySize];

            try
            {
                //Open file and read it line by line until reach a null line.  
                using (StreamReader reader = File.OpenText(file))
                {
                    string lineRead = " ";
                    int i = 0;

                    while ((lineRead = reader.ReadLine()) != null)
                    {
                        //Parse the line read by comma and put each pasrsed word into a new element in the words array
                        string[] words = lineRead.Split(',');
                        //if the line read is blank, initialize a blank object
                        if (words[0] == " ")
                            arrayFromFile[i] = new Restaurant();
                        //if the line read is not blank, put each parsed word into its corresponding restaurant object field
                        else
                            arrayFromFile[i] = new Restaurant(words[0], words[1], Convert.ToInt32(words[2])); 
                        i++;   
                    }    
                } //end using StreamReader
                fileStatus = "successful"; 
            }
            catch (Exception e)
            {
                fileStatus = "unsuccessful";
                Console.WriteLine($"File {file} not found.");   
                Console.WriteLine(e);  
            }
            finally
            {
                Console.WriteLine($"File load {fileStatus}. Returning to main menu.");    
            }
            
            return arrayFromFile; 
        } //end FileToArray method



        //This method loops through the array of Restaurant objects and writes out each row on a new line
        static void PrintArray(Restaurant[] restaurantsArray)
        {
            bool elementsFound = false;

            for (int i = 0; i < restaurantsArray.Length; i ++)
            {
                Console.WriteLine(restaurantsArray[i]);
                if (restaurantsArray[i].Name != " ")
                    elementsFound = true;
            }  

            if (elementsFound == false)
                Console.WriteLine("Empty list of restaurants");
        } //end PrintArray method



        /*This method loops through a 1D array of Restaurant objects looking for an available space.  
          If space is not available 
            Output an error message
          else
            update the first available spot with restaurant name, cuisine, and rating
          Return a 1D array of Restaurant objects
        */
        static Restaurant[] AddRestaurant(Restaurant[] restaurantsArray)
        {
            int indexFound = -1;
            int input;
            bool validInput = false;
             
            //loop through array of Restaurant objects with the assumption that if the Restaurant name is empty, the other values in for that Restaurant are also empty
            for (int i = 0; i < restaurantsArray.Length; i++)
            {
                    //only update indexFound value if it has not already been changed so that we force the first available space to get updated later
                    if (restaurantsArray[i].Name == " " && indexFound == -1)
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
                restaurantsArray[indexFound].Name = Console.ReadLine();
                Console.Write("Enter the type of cuisine:");
                restaurantsArray[indexFound].Cuisine = Console.ReadLine();
                Console.Write("Enter a rating:");
                do
                {
                    input = Convert.ToInt32(Console.ReadLine());
                    if (input >= 0 && input <= 5)
                    {   
                        validInput = true;  

                        restaurantsArray[indexFound].setRating(input);  
                    }
                    else 
                        Console.Write("Star rating must be between 0 and 5.  Please re-enter a rating:");        
                } while (!validInput);
                Console.WriteLine($"Restaurant added at index {indexFound}");
            }

            return restaurantsArray;
        } //end AddRestaurant method



        /*This method prompts the user for a restaurant name and then loops through the rows of a 1D array of Restaurant objects looking for a match.
          If a match is found
            prompt user for what they want to update
            do 
                obtain category from user
                if name
                    update name property
                if cuisine
                    update cuisine property
                if rating
                    check valid range
                        update rating field
                if other
                    Write out error message
            while category flag set to false
          else
            Write out an error message
          Return a 1D array Restaurant objects
        */
        static Restaurant[] UpdateRestaurant(Restaurant[] restaurantsArray)
        {
            string name;
            int restaurantIndex = -1;

            Console.Write("Please enter the restaurant to update:");
            name = Console.ReadLine();
 
            //loop through row of array and set restauntIndex if a matching name is found
            for (int i = 0; i < restaurantsArray.Length; i++)
            {
                if (restaurantsArray[i].Name == name)
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
                    Console.Write("Enter one of the following to update: name, cuisine, rating:");
                    string input = (Console.ReadLine()).ToLower();
                    switch (input)
                    {
                        case "name":
                            Console.Write("Enter new name:");
                            restaurantsArray[restaurantIndex].Name = Console.ReadLine();
                            inputValid = true;
                            break;
                        case "cuisine":
                            Console.Write("Enter new cuisine:");
                            restaurantsArray[restaurantIndex].Cuisine = Console.ReadLine();
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
                                    restaurantsArray[restaurantIndex].setRating(newRating);
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

            return restaurantsArray;
        }  //end UpdateRestaurant method       



        /*This method promts the user for a restaurant name and then loops through the rows of a 1D array of Restaurant objects looking for a match.
          If a match is found
            replace the object at that index with a blank object
          else
            Write out an error message
          Return a 1D array of Restaurant objects
        */
        static Restaurant[] DeleteRestaurant(Restaurant[] restaurantsArray)
        {
            string name;
            int restaurantIndex = -1;

            Console.Write("Please enter the restaurant to delete:");
            name = Console.ReadLine();
 
            //loop through row of array and set restauntIndex if a matching name is found
            for (int i = 0; i < restaurantsArray.Length; i++)
            {
                if (restaurantsArray[i].Name == name)
                {
                    restaurantIndex = i;    
                }
            }

            //if a matching name is found, instantiate a blank Restaurant object
            if (restaurantIndex != -1)
            {
                restaurantsArray[restaurantIndex] = new Restaurant();
                
            }
            //if a matching name is not found, output a message
            else
                Console.WriteLine("Restaurant not found.");

            return restaurantsArray;      
        } //end DeleteRestaurant method


        //This method takes in a 1D array of Restaurants objects and a file, loops through the array, and writes each object to a file
        static void ArrayToFile(Restaurant[] restaurantsArray, string file)
        {   
            string writeStatus = "";
            try
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    for (int i = 0; i < restaurantsArray.Length; i++)
                    {
                        writer.WriteLine(restaurantsArray[i]);
                    } 
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
