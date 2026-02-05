using System;
using System.Dynamic;

namespace RestaurantObjects
{
    class Restaurant
    {
        private string cuisine;
        //the question mark allows for rating to have a null value
        private int? rating;

        public string Name 
            { get; set; }  

        //constructor for Restaurant object that takes 3 parameters
        public Restaurant(string restaurantName, string restaurantCuisine, int? restaurantRating)
        {
            Name = restaurantName;
            cuisine = restaurantCuisine;
            rating = restaurantRating;
        }

        //constructor for empty Restaurant object
        public Restaurant()
        {
            Name = " ";
            cuisine = " ";
            rating = null;
        }

        public string Cuisine
        {
            get {return cuisine;}
            set {cuisine = value;}
        }

        public int? getRating()
        {
            return rating;
        }

        public void setRating(int newRating)
        {
            rating = newRating;
        }
        public override string ToString()
        {
            if (Name == " ")
                return (" ");
            else
                return ($"{Name},{cuisine},{rating}");
        }
    }   //end class
} //end namespace