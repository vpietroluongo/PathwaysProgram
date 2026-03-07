namespace FlashCardApi.Models;

public class FlashCardItem
{
    public long Id { get; set; }
    public string? Question { get; set; }

    public string? Answer { get; set; }
    public bool IsCorrect { get; set; }
}
