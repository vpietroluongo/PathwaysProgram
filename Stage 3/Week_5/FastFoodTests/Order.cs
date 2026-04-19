public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<string> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public bool IsPrepared { get; set; }
    public DateTime OrderTime { get; set; }
}
