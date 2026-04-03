namespace MovieApp;

public class MovieAccess
{
    private readonly List<Movie> _movies = new();

    public void AddMovie(Movie movie)
    {
        movie.Id = _movies.Count + 1;
        _movies.Add(movie);
    }

    public List<Movie> GetAllMovies()
    {
        return _movies.ToList();
    }

    public Movie? GetMovieById(int id)
    {
        return _movies.FirstOrDefault(m => m.Id == id);
    }

    public List<Movie> GetMoviesByGenre(string genre)
    {
        return _movies.Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Movie> GetMoviesByYear(int year)
    {
        return _movies.Where(m => m.ReleaseYear == year).ToList();
    }

    public bool DeleteMovie(int id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return false;

        return _movies.Remove(movie);
    }

    public int GetTotalMovieCount()
    {
        return _movies.Count;
    }
}