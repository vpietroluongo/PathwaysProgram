namespace MusicCollectionInterface
{
    class Album : Songs, IShuffleSongs
    {
        public string Artist  //property
            { get; set; }
        
        public string AlbumName //property
            { get; set; }

        public Album(List<string> songList, string newArtist, string newAlbum) //constructor
        {
           SongTitles = songList;
           Artist = newArtist;
           AlbumName = newAlbum; 
        }

        public void ShuffleSongs()  //implement interface method
        {
            string[] songArray = SongTitles.ToArray();    //copy list to array because Random.Shuffle() does not accept Lists
            Random shuffler = new Random();               //create instance of Random class
            shuffler.Shuffle(songArray);                  //invoke Shuffle method on Random object to randomly sort the songs in the array
            SongTitles = songArray.ToList();              //copy array back to a list
        }

        public override string ToString()  //write the album name and artist first followed by the inherited ToString from Songs parent class
        {
            return $"Songs from the album {AlbumName} by {Artist}: \n" + base.ToString();
        }
    }  //end class
} //end namespace