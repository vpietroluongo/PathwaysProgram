// using System;
// using System.Threading.Tasks;

// Console.WriteLine("Recipe Timer started");
// var boilWater = CookStep("Boil water", 8);
// Console.WriteLine("You can still type while waiting...");
// await boilWater;
// await CookStep("Cook pasta", 12);
// await CookStep("Make sauce", 6);
// // var step4 = CookStep("Chop vegetables", 5);
// // var step5 = CookStep("Preheat oven", 10);
// // await Task.WhenAll(step4, step5);
// await RunParallelSteps();
// Console.WriteLine("Recipe Complete!");

// // TODO: Add your async methods and calls here
// async Task CookStep(string stepName, int seconds)
// {
//     Console.WriteLine($"Starting: {stepName}");
//     await Task.Delay(seconds * 1000);
//     Console.WriteLine($"Finished: {stepName}");
// }

// async Task RunParallelSteps()
// {
//     Console.WriteLine("Starting parallel steps");
//     var step4 = CookStep("Chop vegetables", 5);
//     var step5 = CookStep("Preheat oven", 10);
//     await Task.WhenAll(step4, step5);
//     Console.WriteLine("Parallel steps completed!");
// }

// Console.WriteLine("Program finished.");
// Console.ReadLine();



using System;
using System.Threading.Tasks;

Console.WriteLine("Recipe Timer started");

await CookStep("Boil water", 8);
await CookStep("Cook pasta", 12);
await CookStep("Make sauce", 6);

Console.WriteLine("Recipe complete!");

Console.WriteLine("\nPress ENTER to exit...");
Console.ReadLine();

async Task CookStep(string stepName, int seconds)
{
    Console.WriteLine($"Starting: {stepName}");
    await Task.Delay(seconds * 1000);
    Console.WriteLine($"Finished: {stepName}");
}

// Bonus: Running steps in parallel
async Task RunParallelSteps()
{
    Console.WriteLine("\nStarting parallel steps...");

    var chop = CookStep("Chop vegetables", 5);
    var preheat = CookStep("Preheat oven", 10);

    await Task.WhenAll(chop, preheat);

    Console.WriteLine("Parallel steps completed!");
}