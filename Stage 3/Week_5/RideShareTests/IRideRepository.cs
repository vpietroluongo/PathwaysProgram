public interface IRideRepository
{
    Task<Ride?> GetRideAsync(int rideId);
    Task AddRideAsync(Ride ride);
    Task UpdateRideAsync(Ride ride);
    Task<Driver?> FindAvailableDriverAsync(string pickupLocation);
    Task<bool> IsDriverAssignedAsync(int rideId);
}
