using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

var movies = new List<Movie>
{
    new Movie { Title = "Tangled", Year = 2010, Rating = 7.7 },
    new Movie { Title = "Frozen", Year = 2013, Rating = 7.4 },
    new Movie { Title = "The Little Mermaid", Year = 1989, Rating = 7.6 }
};


const string FilePath = "disney_movies.json";

// TODO: Complete the 4 activities below

//Activity 1: Save movies synchronously
SaveMovieSync();

void SaveMovieSync()
{
    string json = JsonSerializer.Serialize(movies, new JsonSerializerOptions {WriteIndented = true});
    File.WriteAllText(FilePath, json);
    Console.WriteLine("Movies saved synchronously.");
}


//Activity 2: Load movies synchronously
LoadMoviesSync();

void LoadMoviesSync()
{
    if (!File.Exists(FilePath))
    {
        Console.WriteLine("No file found.");
        return;
    }

    string json = File.ReadAllText(FilePath);
    var loadedMovies = JsonSerializer.Deserialize<List<Movie>>(json);

    Console.WriteLine("Movies loaded synchronously:");
    foreach (var m in loadedMovies ?? new List<Movie>())
    {
        Console.WriteLine($"{m.Title} ({m.Year}) - Rating: {m.Rating}");
    }
}


//Activity 3: Save movies asynchronously
await SaveMovieAsync();

async Task SaveMovieAsync()
{
    string json = JsonSerializer.Serialize(movies, new JsonSerializerOptions {WriteIndented = true});
    await File.WriteAllTextAsync(FilePath, json);
    Console.WriteLine("Movies saved asynchronously.");    
}


//Activity 4: Load movies asynchronously
await LoadMovieAsync();

async Task LoadMovieAsync()
{
    if (!File.Exists(FilePath))
    {
        Console.WriteLine("No file found.");
        return;
    }

    var json = await File.ReadAllTextAsync(FilePath);
    var loadedMovies = JsonSerializer.Deserialize<List<Movie>>(json);
    Console.WriteLine("Movies loaded asynchronously.");

    foreach (var m in loadedMovies ?? new List<Movie>())
    {
        Console.WriteLine($"{m.Title} ({m.Year}) - Rating: {m.Rating}");
    }
}
class Movie
{
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public double Rating { get; set; }
}