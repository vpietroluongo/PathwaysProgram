public record Ride(int Id, int RiderId, int DriverId, string PickupLocation, string DropoffLocation, 
                   decimal Fare, DateTime RequestedTime, bool IsCompleted = false);