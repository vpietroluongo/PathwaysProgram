using System;

namespace MusicCollection 
{
    class Album : Music
    {
        public string Artist
            { get; set;}
        
        public string AlbumName
            { get; set;}
        
        public override void ShuffleSongs()
        {
            Random shuffler = new Random();
            shuffler.Shuffle(SongTitles);
        }

        public override string ToString()
        {
            string newString = "";

            for (int i = 0; i < SongTitles.Length; i++)
            {
                newString += $"{i+1}. {SongTitles[i]} \n";
            }
            return newString;
        }

    }  //end  class
}  //end namespace