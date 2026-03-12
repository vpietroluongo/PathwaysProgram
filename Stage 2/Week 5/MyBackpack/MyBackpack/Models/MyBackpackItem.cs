namespace MyBackpack.Models;
public class MyBackpackItem
{
    public long Id { get; set; }
    public string? GearName { get; set; }
    public string? Category { get; set; }
    public double Weight { get; set; }
    public string WeightUnit { get; set; }
    public string IsConsumable { get; set; }
    public string IsWorn { get; set; }
    //public bool IsConsumable { get; set; }
    //public bool IsWorn { get; set; }
}

