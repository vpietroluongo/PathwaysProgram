using System;

namespace RestaurantInheritance
{
    class FoodTruck : Restaurant   //derived class inheriting from Restaurant
    {
        public string CurrentLocation
            { get; set;}
        
        public FoodTruck(string restaurantName, string restaurantCuisine, int? restaurantRating, string newLocation) : base(restaurantName, restaurantCuisine, restaurantRating)
        {
            CurrentLocation = newLocation;
        }

        public FoodTruck() : base()
        {
            CurrentLocation = " ";
        }

        public override string ToString()
        {
            return ($"{base.ToString()},{CurrentLocation}"); 
        }
        
    }   //end class
}   //end namespace