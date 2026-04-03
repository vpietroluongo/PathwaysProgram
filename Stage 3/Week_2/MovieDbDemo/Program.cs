using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MovieDbDemo
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string Director { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Movie> Movies { get; set; } = new();
    }

    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Genre> Genres => Set<Genre>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("MovieDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Genre)
                .WithMany(g => g.Movies)
                .HasForeignKey(m => m.GenreId);
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            using var context = new MovieContext();
            SeedData(context);

            //Activity 1: List All Movies with Genre
            Console.WriteLine("\n=== Activity 1 ===");
            var allMovies = context.Movies
                .Include(m => m.Genre)
                .ToList();
            foreach (var movie in allMovies)
            {
                Console.WriteLine($"{movie.Title}, {movie.ReleaseYear}, {movie.Director}, {movie.Rating}/10, {movie.Genre.Name}");
            }

            Console.WriteLine("Movie Database loaded successfully!");


            //Activity 2: Find Movie by Title
            Console.WriteLine("\n=== Activity 2 ===");
            var inceptionInfo = context.Movies
                .Include(m => m.Genre)
                .FirstOrDefault(m => m.Title == "Inception");
            if (inceptionInfo != null)
            {
                Console.WriteLine($"Title: {inceptionInfo.Title}");
                Console.WriteLine($"     Release date: {inceptionInfo.ReleaseYear}");
                Console.WriteLine($"     Director: {inceptionInfo.Director}");
                Console.WriteLine($"     Rating: {inceptionInfo.Rating}/10");
                Console.WriteLine($"     Genre: {inceptionInfo.Genre.Name}");
            }

            //Activity 3: Add a New Movie
            Console.WriteLine("\n=== Activity 3 ===");
            var dramaGenre = context.Genres
                .First(g => g.Name == "Drama");

            var newMovie = new Movie
            {
                Title = "Oppenheimer",
                ReleaseYear = 2023,
                Director = "Christopher Nolan",
                Rating = 8.4,
                GenreId = dramaGenre.Id
            };
            context.Movies.Add(newMovie);
            await context.SaveChangesAsync();

            var newMovieCheck = context.Movies
                .Include(m => m.Genre)
                .FirstOrDefault(m => m.Title == "Oppenheimer");
            if (newMovieCheck != null)
            {
                                Console.WriteLine($"Title: {newMovieCheck.Title}");
                Console.WriteLine($"     Release date: {newMovieCheck.ReleaseYear}");
                Console.WriteLine($"     Director: {newMovieCheck.Director}");
                Console.WriteLine($"     Rating: {newMovieCheck.Rating}/10");
                Console.WriteLine($"     Genre: {newMovieCheck.Genre.Name}");
            }


            //Activity 4: Update Movie Rating
            Console.WriteLine("\n=== Activity 4 ===");
            var darkKnightInfo = context.Movies
                .FirstOrDefault(m => m.Title == "The Dark Knight");
            if (darkKnightInfo != null)
            {  
                darkKnightInfo.Rating = 9.2;
                context.SaveChanges();

                var updateMovieCheck = context.Movies
                    .FirstOrDefault(m => m.Title == "The Dark Knight");
                if (updateMovieCheck != null)
                {
                    Console.WriteLine($"{updateMovieCheck.Title} is rated {updateMovieCheck.Rating}/10");
                }
            }


            //Activity 5: Remove a Movie
            Console.WriteLine("\n=== Activity 5 ===");
            var parasiteInfo = context.Movies
                .FirstOrDefault(m => m.Title == "Parasite");
            if (parasiteInfo != null)
            {
                context.Movies.Remove(parasiteInfo);
                await context.SaveChangesAsync();

                var removeMovieCheck = context.Movies
                    .FirstOrDefault(m => m.Title == "Parasite");
                if (removeMovieCheck == null)
                {
                    Console.WriteLine("Parasite successfully removed.");
                }
            }
        }

        static void SeedData(MovieContext context)
        {
            if (context.Movies.Any()) return;

            var action = new Genre { Name = "Action" };
            var comedy = new Genre { Name = "Comedy" };
            var drama = new Genre { Name = "Drama" };
            var scifi = new Genre { Name = "Sci-Fi" };
            var thriller = new Genre { Name = "Thriller" };

            context.Genres.AddRange(action, comedy, drama, scifi, thriller);
            context.SaveChanges();

            var movies = new List<Movie>
            {
                new Movie { Title = "Inception", ReleaseYear = 2010, Director = "Christopher Nolan", Rating = 8.8, GenreId = scifi.Id },
                new Movie { Title = "The Dark Knight", ReleaseYear = 2008, Director = "Christopher Nolan", Rating = 9.0, GenreId = action.Id },
                new Movie { Title = "Interstellar", ReleaseYear = 2014, Director = "Christopher Nolan", Rating = 8.7, GenreId = scifi.Id },
                new Movie { Title = "Parasite", ReleaseYear = 2019, Director = "Bong Joon-ho", Rating = 8.5, GenreId = drama.Id },
                new Movie { Title = "Dune: Part Two", ReleaseYear = 2024, Director = "Denis Villeneuve", Rating = 8.6, GenreId = scifi.Id }
            };

            context.Movies.AddRange(movies);
            context.SaveChanges();
            Console.WriteLine("Database seeded with 5 movies.");
        }
    }
}