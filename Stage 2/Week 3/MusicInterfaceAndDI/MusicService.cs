using System;

namespace MusicInterfaceAndDI;

class MusicService : IShuffler
{
    private readonly IShuffler _shuffler;

    public MusicService(IShuffler shuffler)
    {
        _shuffler = shuffler;
    }

    public void ShuffleSongs()
    {
        _shuffler.ShuffleSongs();
    }
}
