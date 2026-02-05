namespace MusicCollectionInterface
{
    class Songs
    {
        public List<string> SongTitles  //property
            { get; set; }

        public Songs()   //constructor
        {
            SongTitles = new List<string>();
        } 

        public void AddSong(string newSong)  //method to add one song
        {
            SongTitles.Add(newSong);
        }

        public void AddSongs(List<string> newSongs)  //method to add multiple songs
        {
            foreach (string song in newSongs) 
                SongTitles.Add(song);
        }

        public override string ToString()  //writes out each song on a new line
        {
            string newString = "";

            for (int i = 0; i < SongTitles.Count; i++)
            {
                newString = newString + $"{i+1}. {SongTitles[i]} \n";
            }
            return newString;
        }
    }  //end class
} //end namespace