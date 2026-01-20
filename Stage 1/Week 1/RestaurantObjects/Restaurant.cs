using System;
using System.Dynamic;

namespace RestaurantObjects
{
    class Restaurant
    {
        private string name;
        private string cuisine;
        //the question mark allows for rating to have a null value
        private int? rating;

        //constructor for Restaurant object that takes 3 parameters
        public Restaurant(string restaurantName, string restaurantCuisine, int? restaurantRating)
        {
            name = restaurantName;
            cuisine = restaurantCuisine;
            rating = restaurantRating;
        }

        //constructor for empty Restaurant object
        public Restaurant()
        {
            name = " ";
            cuisine = " ";
            rating = null;
        }
        public string Name
        //{get; set;}   //need to figure out why this wasn't working
        {
            get {return name;}
            set {name = value;}
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
            if (name == " ")
                return (" ");
            else
                return ($"{name},{cuisine},{rating}");
        }
    }   //end class
} //end namespace