using System;
using System.Reflection;
using MovieApp;

namespace MovieApp.Tests;

[TestClass]
public class MovieAccessTests
{
    private readonly MovieAccess _movieAccess;

    public MovieAccessTests()
    {
        _movieAccess = new MovieAccess();
    }

    [TestMethod]
    public void AddMovie_ShouldIncreaseCountByOne()
    {
        //Arrange
        var movie = new Movie
        {
            Title = "Project Hail Mary",
            Genre = "Sci-Fi",
            ReleaseYear = 2026,
            Rating = 8.8m
        };

        //Act
        _movieAccess.AddMovie(movie);
        int count = _movieAccess.GetTotalMovieCount();

        //Assert
        Assert.AreEqual(1, count);
    }

    [TestMethod]
    public void GetMovieById_ShouldReturnCorrectMovie()
    {
        //Arrange
        var movie = new Movie
        {
            Title = "Project Hail Mary",
            Genre = "Sci-Fi",
            ReleaseYear = 2026,
            Rating = 8.8m
        };
        _movieAccess.AddMovie(movie);

        //Act
        var result = _movieAccess.GetMovieById(movie.Id);

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(result.Id, movie.Id);
        Assert.AreEqual(result.Title, movie.Title);
        Assert.AreEqual(result.Genre, movie.Genre);
        Assert.AreEqual(result.ReleaseYear, movie.ReleaseYear);
        Assert.AreEqual(result.Rating, movie.Rating);
    }

    [TestMethod]
    public void GetMoviesByGenre_ShouldReturnOnlyMoviesOfThatGenre()
    {
        //Arrange
        _movieAccess.AddMovie(new Movie
        {
            Title = "Project Hail Mary",
            Genre = "Sci-Fi",
            ReleaseYear = 2026,
            Rating = 8.8m
        });
        _movieAccess.AddMovie(new Movie
        {
            Title = "The Dark Knight",
            Genre = "Action",
            ReleaseYear = 2008,
            Rating = 9.0m
        });
        _movieAccess.AddMovie(new Movie
        {
            Title = "Interstellar",
            Genre = "Sci-Fi",
            ReleaseYear = 2014,
            Rating = 8.6m
        });

        //Act
        var result = _movieAccess.GetMoviesByGenre("Sci-Fi");

        //Assert
        //Assert.AreEqual(2, result.Count);
        Assert.HasCount(2, result);
        Assert.IsTrue(result.All(m => m.Genre.Equals("Sci-fi", StringComparison.OrdinalIgnoreCase)));
    }

    [TestMethod]
    public void DeleteMovie_ShouldReturnTrueWhenMovieExistsAndIsDeleted()
    {
        //Arrange
        var movie = new Movie
        {
            Title = "Project Hail Mary",
            Genre = "Sci-Fi",
            ReleaseYear = 2026,
            Rating = 8.8m
        };
        _movieAccess.AddMovie(movie);

        //Act
        bool deleted = _movieAccess.DeleteMovie(movie.Id);

        //Assert
        Assert.IsTrue(deleted);
        Assert.AreEqual(0, _movieAccess.GetTotalMovieCount());
    }

    [TestMethod]
    public void GetAllMovies_ShouldReturnCopy()
    {
        //Arrange
        _movieAccess.AddMovie(new Movie
        {
            Title = "Project Hail Mary",
            Genre = "Sci-Fi",
            ReleaseYear = 2026,
            Rating = 8.8m
        });
        _movieAccess.AddMovie(new Movie
        {
            Title = "The Dark Knight",
            Genre = "Action",
            ReleaseYear = 2008,
            Rating = 9.0m
        });
        _movieAccess.AddMovie(new Movie
        {
            Title = "Interstellar",
            Genre = "Sci-Fi",
            ReleaseYear = 2014,
            Rating = 8.6m
        });

        //Act
        var result = _movieAccess.GetAllMovies();

        //Assert
        Assert.HasCount(3, result);
        Assert.IsTrue(result.Select(m => m.Title).Contains("Project Hail Mary"));
        Assert.IsTrue(result.Select(m => m.Title).Contains("The Dark Knight"));
        Assert.IsTrue(result.Select(m => m.Title).Contains("Interstellar"));
    }

    [TestMethod]
    public void GetMovieByYear_ShouldReturnOnlyMoviesReleasedThatYear()
    {
        //Arrange
        _movieAccess.AddMovie(new Movie
        {
            Title = "Project Hail Mary",
            Genre = "Sci-Fi",
            ReleaseYear = 2026,
            Rating = 8.8m
        });
        _movieAccess.AddMovie(new Movie
        {
            Title = "The Dark Knight",
            Genre = "Action",
            ReleaseYear = 2008,
            Rating = 9.0m
        });

        //Act
        var result = _movieAccess.GetMoviesByYear(2026);

        //Assert
        Assert.HasCount(1, result);
        Assert.IsTrue(result.All(m => m.ReleaseYear == 2026));
    }
}
