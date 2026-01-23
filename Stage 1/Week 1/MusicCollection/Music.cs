using System;

namespace MusicCollection
{
    class Music
    {
        public string[] SongTitles
            { get; set;}

        public Music()
        {
            string[] SongTitles = new string[5];
            for (int i = 0; i < SongTitles.Length; i++)
            {
                SongTitles[i] = " ";
            }    
        }
        public virtual void ShuffleSongs()
        {
            Random shuffler = new Random();
            shuffler.Shuffle(SongTitles);
        }

        public override string ToString()
        {
            string newString = "";

            for (int i = 0; i < SongTitles.Length; i++)
            {
                newString = newString + $"{i+1}. {SongTitles[i]} \n";
            }
            return newString;
        }

    }  //end class
}   //end namespace