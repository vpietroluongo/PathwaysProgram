//public abstract record RideRequest(int RiderId);
public record RideRequest(int RiderId);

public record BookRideRequest(int RiderId, string PickupLocation, string DropoffLocation, decimal EstimatedFare)
    : RideRequest(RiderId);

public record CancelRideRequest(int RiderId, int RideId, string Reason)
    : RideRequest(RiderId);

public record GetRideStatusRequest(int RiderId, int RideId)
    : RideRequest(RiderId);
