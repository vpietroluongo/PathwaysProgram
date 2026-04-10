public class RocketController
{
    private readonly IRocketEngine _engine;

    public RocketController(IRocketEngine engine)
    {
        _engine = engine;
    }

    public async Task<bool> PrepareForLaunchAsync(string rocketId)
    {
        bool enginesOk = await _engine.CheckEnginesAsync(rocketId);
        if (!enginesOk)
            return false;

        int fuel = await _engine.GetFuelLevelAsync(rocketId);
        if (fuel < 80)
            return false;

        await _engine.LaunchAsync(rocketId);
        
        return true;
    }

    public async Task<string> GetMissionStatusAsync(string rocketId)
    {
        int fuel = await _engine.GetFuelLevelAsync(rocketId);
        return fuel > 50 ? "Go for launch!" : "Refuel needed!";
    }
}
