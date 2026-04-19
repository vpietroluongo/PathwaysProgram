public class RideService
{
    private readonly IRideRepository _repository;
    private readonly INotificationService _notificationService;

    public RideService(IRideRepository repository, INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public async Task<object> ProcessRequestAsync(RideRequest request)
    {
        return request switch
        {
            BookRideRequest book => await HandleBookRideAsync(book),
            CancelRideRequest cancel => await HandleCancelRideAsync(cancel),
            GetRideStatusRequest status => await HandleGetStatusAsync(status),
            _ => throw new ArgumentException("Unsupported ride request type")
        };
    }

    private async Task<bool> HandleBookRideAsync(BookRideRequest request)
    {
        var driver = await _repository.FindAvailableDriverAsync(request.PickupLocation);
        if (driver == null || !driver.IsAvailable)
            return false;

        var ride = new Ride(
            Id: 0,
            RiderId: request.RiderId,
            DriverId: driver.Id,
            PickupLocation: request.PickupLocation,
            DropoffLocation: request.DropoffLocation,
            Fare: request.EstimatedFare,
            RequestedTime: DateTime.UtcNow
        );

        await _repository.AddRideAsync(ride);
        await _notificationService.SendRideConfirmationAsync(
            "555-1234", request.PickupLocation, request.DropoffLocation); // simplified phone

        await _notificationService.SendDriverAssignedNotificationAsync(
            "555-1234", driver.Name, driver.Vehicle);

        return true;
    }

    private async Task<bool> HandleCancelRideAsync(CancelRideRequest request)
    {
        var ride = await _repository.GetRideAsync(request.RideId);
        if (ride == null || ride.IsCompleted)
            return false;

        var updatedRide = ride with { IsCompleted = true }; // records are immutable
        await _repository.UpdateRideAsync(updatedRide);

        await _notificationService.SendRideCancelledNotificationAsync(
            "555-1234", request.Reason);

        return true;
    }

    private async Task<string> HandleGetStatusAsync(GetRideStatusRequest request)
    {
        var ride = await _repository.GetRideAsync(request.RideId);
        return ride?.IsCompleted == true ? "Completed" : "In Progress";
    }
}
