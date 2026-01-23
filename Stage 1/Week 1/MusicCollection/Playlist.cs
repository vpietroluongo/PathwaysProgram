using System;
using System.Runtime.InteropServices;

namespace MusicCollection
{
    class Playlist : Music
    {
        public string[] Artists
            { get; set;}
        
        public Playlist() : base()
        {
            string[] Artists = new string[5];
            for (int i = 0; i < Artists.Length; i++)
            {
                Artists[i] = " ";
            }    
        }
        public override void ShuffleSongs()
        {
            int[] nums = new int[SongTitles.Length];
            string[] tempArraySongs = new string[SongTitles.Length];
            string[] tempArrayArtists = new string[Artists.Length];
            

            //the temp arrays will hold the initial values in their initial places
            //the nums array will hold the initial index; the element value will be equal the the index value
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = i;
                tempArraySongs[i] = SongTitles[i];
                tempArrayArtists[i] = Artists[i];
            }

            //shuffle the nums array so we know where each index should wind up; Shuffle() did not take two arguments
            Random shuffler = new Random();
            shuffler.Shuffle(nums);

            /*loop through nums. i will be the new index of the song and artist values. num[i]  will be the value of the original index.  
                    Move what was at the original index to the new index */
            for (int i = 0; i < nums.Length; i++)
            {
                SongTitles[i] = tempArraySongs[nums[i]];
                Artists[i] = tempArrayArtists[nums[i]];      
            }
        }


        public override string ToString()
        {
            string newString = "";

            for (int i = 0; i < SongTitles.Length; i++)
            {
                newString += $"{i+1}. {SongTitles[i]} by {Artists[i]} \n";
            }
            return newString;
        }

    }  //end class
}  //end namespace