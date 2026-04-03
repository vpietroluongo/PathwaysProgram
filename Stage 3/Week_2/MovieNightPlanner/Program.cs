using System;
using System.Globalization;

namespace MoveNightPlanner;

class Pogram
{
    static void Main(string[] args)
    {
        bool runAgain = true;

        while (runAgain == true)
        {
            Console.Write("How many movies do you want to add (1-6): ");
            string input = Console.ReadLine();
            int number;
            while (!int.TryParse(input, out number) || number < 1 || number > 6)
            {
                Console.Write("Invalid input.  Enter a whole number between 1 and 6: ");
                input = Console.ReadLine();
            }
            
            string[] titles = new string[number];
            int[] ratings = new int[number];
            string[] genres = new string[number];
            for (int i = 0; i < number; i++)
            {
                Console.Write("Enter a movie title: ");
                titles[i] = Console.ReadLine();
                while (titles[i] == "")
                {
                    Console.Write("Invalid input.  Please enter a title: ");
                    titles[i] = Console.ReadLine();
                }

                Console.Write("Enter the genre: ");
                genres[i] = Console.ReadLine();
                while (genres[i] == "")
                {
                    Console.Write("Invalid input.  Please enter a genre: ");
                    genres[i] = Console.ReadLine();
                }

                Console.Write("Enter the rating (1-10): ");
                input = Console.ReadLine();
                while (!int.TryParse(input, out ratings[i]) || ratings[i] < 1 || ratings[i] > 10)
                {
                    Console.Write("Invalid input.  Enter a whole number between 1 and 10: ");
                    input = Console.ReadLine();
                }
            }

            int maxIndex = FindMaxIndex(ratings);
            double averageRating = CalculateAverage(ratings);

            for(int i = 0; i < titles.Length; i++)
            {
                Console.WriteLine($"Title: {titles[i]} | Rating: {ratings[i]} | Genre: {genres[i]}");
            }
            Console.WriteLine($"The highest rated movie is {titles[maxIndex]} with a rating of {ratings[maxIndex]}");
            Console.WriteLine($"The average rating it {averageRating:F1}");

            var genreCount = genres
                .GroupBy(g => g)
                .Select(g => new
                {
                   Genre = g.Key,
                   Count = g.Count() 
                });
            foreach (var genre in genreCount)
            {
                Console.WriteLine($"Genre: {genre.Genre} | Count: {genre.Count}");
            }

            Console.Write("Do you want to run again? (yes/no): ");
            input = (Console.ReadLine()).ToLower();
            bool validInput = false;
            while (!validInput)
            {  
                if (input == "no")
                {
                    validInput = true;
                    runAgain = false;
                }
                else if (input == "yes")
                {
                    validInput = true;
                }
                else
                {
                    Console.Write("Invalid input, enter yes or no: ");
                    input = Console.ReadLine();
                }
            }
        }
    }

    static int FindMaxIndex(int[] ratings)
    {
        int max = int.MinValue;
        int index = 0;

        for (int i = 0; i < ratings.Length; i++)
        {
            if (ratings[i] > max)
            {
                max = ratings[i];
                index = i;
            }
        }

        return index;
    }

    static double CalculateAverage(int[] ratings)
    {
        double sum = 0;
        foreach (int rating in ratings)
        {
            sum += rating;
        }
        return sum / ratings.Length;
    }
}
