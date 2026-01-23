using System;
using System.Security.Cryptography.X509Certificates;

namespace MusicCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Music myMusic = new Music();
            // = 10;

            string[] songs = new string[] { "song1", "song2", "song3", "song4", "song5" };

            myMusic.SongTitles = songs;

            foreach(string song in myMusic.SongTitles)
            {
                Console.WriteLine(song);
            }

            myMusic.ShuffleSongs();
            Console.WriteLine("");

            foreach(string song in myMusic.SongTitles)
            {
                Console.WriteLine(song);
            }

            Console.WriteLine(myMusic);

            Playlist myPlaylist = new Playlist();

            myPlaylist.SongTitles = new string[] { "p-song1", "p-song2", "p-song3", "p-song4", "p-song5" };
            myPlaylist.Artists = new string[] {"artist1", "artist2", "artist3", "artist4", "artist5"};


            Console.WriteLine(myPlaylist);

            myPlaylist.ShuffleSongs();
        
            Console.WriteLine("");

            Console.WriteLine(myPlaylist);

        }  //end Main method
    }  //end class
}  //end namespace
