using System;
using System.Globalization;
using System.Transactions;
using Microsoft.VisualBasic.FileIO;

namespace RestaurantInheritance
{
    class Program
    {
        //This program reads a comma-delimitted file with information about up to 25 restaurant names, their type of cuisine, star ratings, and if it's a food truck, their current location,
        //and lets the user modify and save that info
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
                        AddRestaurant(restaurants);
                        break;
                    case "R":
                        Console.WriteLine("In R area");
                        PrintArray(restaurants);
                        break;
                    case "U":
                        Console.WriteLine("In U area");
                        UpdateRestaurant(restaurants);
                        break;
                    case "D":
                        Console.WriteLine("In D area");
                        DeleteRestaurant(restaurants);
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
                        if (words.Length == 4)
                        {  
                            //if the line read has 4 words, it is a food truck, so put each parsed word into its corresponding food truck object field
                            arrayFromFile[i] = new FoodTruck(words[0], words[1], Convert.ToInt32(words[2]),words[3]);
                        }   
                        else 
                        {  
                            //if the line read is blank, initialize a blank restaurant object
                            if (words[0] == " ")
                                arrayFromFile[i] = new Restaurant();
                            //if the line read is not blank, put each parsed word into its corresponding restaurant object field
                            else
                                arrayFromFile[i] = new Restaurant(words[0], words[1], Convert.ToInt32(words[2]));
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
                Console.WriteLine(e);  
            }
            finally
            {
                Console.WriteLine($"File load {fileStatus}. Returning to main menu.");    
            }
            
            return arrayFromFile; 
        } //end FileToArray method



        //This method loops through the array of Restaurant objects and writes out each element on a new line
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
            prompt user asking if the new restaurant if a food truck
            if no
                update the first available spot with restaurant name, cuisine, and rating
            else if yes
                create food truck object
                update food truck object with name, cuisine, rating, and location
                update restaurant object with food truck object
            else
                invalid input error message
        */
        static void AddRestaurant(Restaurant[] restaurantsArray)
        {
            int indexFound = -1;
            int input;
            bool validInput = false;
            bool isYesOrNo = false;
             
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
                Console.Write("Is the new restaurant a food truck? Y or N: ");
                do
                {   
                    string isFoodTruck = (Console.ReadLine()).ToUpper();
                    if (isFoodTruck == "N")
                    {
                        isYesOrNo = true;
                        Console.WriteLine(restaurantsArray[indexFound] is FoodTruck);
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
                    }
                    else if (isFoodTruck == "Y")
                    {
                        isYesOrNo = true;
                        FoodTruck newFoodTruck = new FoodTruck();
                        restaurantsArray[indexFound] = newFoodTruck;
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
                        Console.Write("Enter the food truck's current location:");
                        newFoodTruck.CurrentLocation = Console.ReadLine();
                    }
                    else    
                        Console.Write("Invalid input. Please enter Y or N:");
                } 
                while (!isYesOrNo);
                Console.WriteLine($"Restaurant added at index {indexFound}");
            }

            //return restaurantsArray;
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
                if location
                    if food truck
                        update location field
                    else
                        write out not a food truck message
                if other
                    Write out error message
            while category flag set to false
          else
            Write out an error message
        */
        static void UpdateRestaurant(Restaurant[] restaurantsArray)
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
                    Console.Write("Enter one of the following to update: name, cuisine, rating, location:");
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
                        case "location":
                           //Console.WriteLine(restaurantsArray[restaurantIndex].GetType());  //returns RestaurantInheritance.FoodTruck

                            if (restaurantsArray[restaurantIndex] is FoodTruck)
                            {
                                Console.Write("Enter current food truck location:");
                                //restaurantsArray[restaurantIndex].CurrentLocation = Console.ReadLine();  //resulted in error CS1061: 'Restaurant' does not contain a definition for 'CurrentLocation' and no accessible extension method
                                //FoodTruck restaurantsArray[restaurantIndex] = restaurantsArray[restaurantIndex];  //resulted in error CS0270: Array size cannot be specified in a variable declaration (try initializing with a 'new' expression)
                                //FoodTruck foodTruckObj = restaurantsArray[restaurantIndex];   //resulted in error CS0266: Cannot implicitly convert type 'RestaurantInheritance.Restaurant' to 'RestaurantInheritance.FoodTruck'. An explicit conversion exists (are you missing a cast?)
                                //cast the element to type Foodtruck
                                FoodTruck foodTruckObj = (FoodTruck)restaurantsArray[restaurantIndex];  //this statement throws an exception if the element is not a FoodTruck object
                                foodTruckObj.CurrentLocation = Console.ReadLine();  //foodTruckObj is a reference object and is modifying what's in the array
                            }
                            else
                                Console.Write("This restaurant is not a food truck and location does not change. Enter a different option: ");
                            inputValid = true; 
                            break;
                        default:
                            Console.Write("Invalid input.  Enter name, cuisine, rating, or location:");
                            break;
                    }   //end switch 
                } while (!inputValid); 
            } //end if restaurant found
            //if a match is nout found, output a message
            else
                Console.WriteLine("Restaurant is not found.");

            //return restaurantsArray;
        }  //end UpdateRestaurant method       



        /*This method prompts the user for a restaurant name and then loops through the rows of a 1D array of Restaurant objects looking for a match.
          If a match is found
            replace the object at that index with a blank object
          else
            Write out an error message
          **array passed by reference 
        */
        static void DeleteRestaurant(Restaurant[] restaurantsArray)
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

            //return restaurantsArray;      
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
