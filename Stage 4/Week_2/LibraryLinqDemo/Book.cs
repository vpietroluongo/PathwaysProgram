public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int YearPublished { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
}