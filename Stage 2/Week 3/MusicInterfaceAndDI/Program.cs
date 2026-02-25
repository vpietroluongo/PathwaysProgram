using System;

namespace MusicInterfaceAndDI;
class Program
{
    static void Main(string[] args)
    {

        Songs mySongs = new Songs();
        mySongs.AddSong("song1");
        mySongs.AddSong("song2");
        mySongs.AddSong("song3");
        mySongs.AddSong("song4");
        mySongs.AddSong("song5");
       // Console.WriteLine(mySongs);
        List<string> moreSongs = new List<string> { "song6", "song7", "song8" };
        mySongs.AddSongs(moreSongs);
       // Console.WriteLine(mySongs);



        Album myAlbum = new Album(new List<string>() { "Nice to Know You", "Circles", "Wish You Were Here", "Just a Phase", "11am" }, "Incubus", "Morning View");
        //Console.WriteLine(myAlbum);
        myAlbum.AddSongs(new List<string>() { "Warning", "Aqueous Transmission", "Echo" });
      // Console.WriteLine(myAlbum);

        //myAlbum.ShuffleSongs();
       // MusicService musicService1 = new Album(myAlbum);

        //Console.WriteLine(myAlbum);


        IShuffler myPlaylist = new Playlist();
        Playlist myPlaylist = new Playlist();
        ((Playlist)myPlaylist).AddSong("song1");
        ((Playlist)myPlaylist).AddSong("song2");
        ((Playlist)myPlaylist).AddSong("song3");
        ((Playlist)myPlaylist).AddSong("song4");
        ((Playlist)myPlaylist).AddSong("song5");
        ((Playlist)myPlaylist).Artists = new List<string> { "Billy Joel", "The Beatles", "Polyphia", "Animals as Leaders", "Marcin" };
        ((Playlist)myPlaylist).AddSong("Jamming");
        ((Playlist)myPlaylist).AddArtists(new List<string>() { "Bob Marley" });
        //myPlaylist.AddSong("song1");
        //myPlaylist.AddSong("song2");
        //myPlaylist.AddSong("song3");
        //myPlaylist.AddSong("song4");
        //myPlaylist.AddSong("song5");
        //myPlaylist.Artists = new List<string> { "Billy Joel", "The Beatles", "Polyphia", "Animals as Leaders", "Marcin" };
        //myPlaylist.AddSong("Jamming");
        //myPlaylist.AddArtists(new List<string>() { "Bob Marley" });
        Console.WriteLine(myPlaylist);


        MusicService musicService2 = new MusicService(myPlaylist);
        musicService2.ShuffleSongs();
        Console.WriteLine(myPlaylist);

        // myPlaylist.ShuffleSongs();
        // Console.WriteLine(myPlaylist);
    }  //end Main method
}  //end class
      
