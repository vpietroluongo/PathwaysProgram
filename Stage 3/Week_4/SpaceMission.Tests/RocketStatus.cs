public class RocketStatus
{
    public string RocketId { get; set; } = string.Empty;
    public bool EnginesReady { get; set; }
    public int FuelLevel { get; set; }     // percentage
    public bool Launched { get; set; }
}
