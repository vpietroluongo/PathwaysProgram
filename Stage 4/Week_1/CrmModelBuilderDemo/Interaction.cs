public class Interaction
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime InteractionDate { get; set; }
    public string Type { get; set; } = string.Empty; // Call, Email, Meeting, Note
    public string Notes { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }

    public Customer Customer { get; set; } = null!;
}