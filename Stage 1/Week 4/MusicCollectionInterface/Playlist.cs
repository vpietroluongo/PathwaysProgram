namespace MusicCollectionInterface
{
    class Playlist : Songs, IShuffleSongs
    {
        public List<string> Artists   //property
            { get; set;}
        
        public Playlist() : base()  //constructor
        {
            Artists = new List<string>();  
        }

        public void AddArtists(List<string> newArtists) //method to add artists to list
        {
            foreach (string artist in newArtists) 
                Artists.Add(artist);
        }
        public void ShuffleSongs()  //implement interface method
        {
            int[] nums = new int[SongTitles.Count];
            List<string> tempListSongs = SongTitles.ToList(); //the temp lists will hold the list values in their initial places
            List<string> tempListArtists = Artists.ToList();
            
            
            //the nums array will hold the initial index; the element value will be equal the the index value
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = i;
            }

            //shuffle the nums array so we know where each index should wind up; Shuffle() did not take two arguments
            Random shuffler = new Random();
            shuffler.Shuffle(nums);

            /*loop through nums. i will be the new index of the song and artist values. num[i]  will be the value of the original index.  
                    Move what was at the original index to the new index */
            for (int i = 0; i < nums.Length; i++)
            {
                SongTitles[i] = tempListSongs[nums[i]];
                Artists[i] = tempListArtists[nums[i]];      
            }
        }


        public override string ToString()  //method to print each song and its artist on a new line
        {
            string newString = "";

            for (int i = 0; i < SongTitles.Count; i++)
            {
                newString += $"{i+1}. {SongTitles[i]} by {Artists[i]} \n";  
            }
            return newString;
        }
    } //end class
}  //end namespace