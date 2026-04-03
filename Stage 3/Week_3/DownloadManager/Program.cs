using System;
using System.Threading.Tasks;

Console.WriteLine("Download Manager Started\n");

// TODO: Add your async methods and calls here
Console.WriteLine("\n=== Activity 2 ===");
await DownloadFile("report.pdf", 4);
await DownloadFile("image.jpg", 6);
await DownloadFile("data.csv", 3);
Console.WriteLine("All sequential downloads completed!");

Console.WriteLine("\n=== Activity 3 ===");
var download4 = DownloadFile("movie.mp4", 8);
var download5 = DownloadFile("music.mp3", 5);
var download6 = DownloadFile("ebook.pdf", 7);
await Task.WhenAll(download4, download5, download6);
Console.WriteLine("All parallel downloads completed!");

Console.WriteLine("\n=== Activity 4 ===");
int size = await GetFileSize("video.mp4");
Console.WriteLine($"File size is {size}");

Console.WriteLine("\n=== Activity 5 ===");
await DownloadWithProgress("largefile.zip", 10);

Console.WriteLine("\nProgram finished.");
Console.ReadLine();


async Task DownloadFile(string filename, int seconds)
{
    Console.WriteLine($"Starting download: {filename}");
    await Task.Delay(seconds * 1000);
    Console.WriteLine($"Download completed: {filename}");
}

async Task<int> GetFileSize(string filename)
{
    Console.WriteLine($"Getting size of {filename}");
    await Task.Delay(2 * 1000);
    Random random = new Random();
    int size = random.Next(10, 101);
    return size;
}

async Task DownloadWithProgress(string filename, int totalSeconds)
{
    Console.WriteLine($"Starting download with progress: {filename}");

    for (int i = 1; i <= totalSeconds; i++)
    {
        await Task.Delay(1000);
        int percent = (i * 100) / totalSeconds;
        Console.WriteLine($"Downloading {filename}...{percent}% complete");
    }

    Console.WriteLine($"Download completed: {filename}");
}
