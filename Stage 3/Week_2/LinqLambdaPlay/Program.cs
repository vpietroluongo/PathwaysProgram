using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

var people = new List<Person>
{
    new("Mia",    24, "Berlin",   true,  3),
    new("Lucas",  31, "Lisbon",   false, 2),
    new("Aisha",  19, "Toronto",  true,  5),
    new("Noah",   42, "Austin",   true,  4),
    new("Sofia",  28, "Barcelona",false, 2),
    new("Kai",    22, "Seoul",    true,  3)
};

Console.WriteLine("LINQ + Lambda playground ready\n");

var coffeeLovers = people
    .Where(p => p.LikesCoffee)
    .Select(p => p.Name);
Console.WriteLine(string.Join("\n", coffeeLovers.Select(x => $"{x} likes coffee")));


var under25 = people
    .Where(p => p.Age < 25)
    .OrderBy(p => p.Age)
    .Select(p => p.Name);
Console.WriteLine("--People under 25--");
Console.WriteLine(string.Join("\n", under25));


bool fromBerlin = people
    .Any(p => p.City == "Berlin");
Console.WriteLine(fromBerlin ? "yes, someone is from Berlin" : "no, no one is from Berlin");


var moreThan3Hobbies = people
    .Count(p => p.HobbiesCount >= 3);
Console.WriteLine($"There are {moreThan3Hobbies} people with more than 3 hobbies.");


// var shortDescription = people
//     .Select(p => 
//     $"{p.Name} ({p.Age}) - {p.City} - likes coffee {(p.LikesCoffee ? "yes" : "no")}");
// //Console.WriteLine("\n", shortDescription);
// foreach (var person in shortDescription)
// {
//     Console.WriteLine(person);
// }
Console.WriteLine(string.Join("\n", people.Select(p => $"{p.Name} ({p.Age}) - {p.City} - likes coffee {(p.LikesCoffee ? "yes" : "no")}")));


var namesAndLength = people
    .OrderByDescending(p => p.Name.Length)
    .Select(p => $"{p.Name} {p.Name.Length}");
Console.WriteLine(string.Join("\n", namesAndLength));


var youngCoffeeLovers = people
    .OrderBy(p => p.Age)
    .Where(p => p.Age >= 20 && p.Age <= 30 && p.LikesCoffee)
    .Select(p => $"{p.Name} {p.Age}");
Console.WriteLine("-- 20-30 and liek coffee --");
Console.WriteLine(string.Join("\n", youngCoffeeLovers));


var fromEurope = people
    .Where(p => p.City == "Berlin" || p.City == "Lisbon" || p.City == "Barcelona")
    .Select(p => $"{p.Name} is from Europe");
Console.WriteLine(string.Join("\n", fromEurope));


bool moreThan4Hobbies = people
    .Any(p => p.HobbiesCount > 4);
Console.WriteLine($"There {(moreThan4Hobbies ? "is" : "is not")} someone with more than 4 hobbies.");


var uppercaseNames = people
    .Select(p => p.Name.ToUpper());
Console.WriteLine("-- Names in uppercase --");
Console.WriteLine(string.Join("\n", uppercaseNames));


var uniqueCities = people
    .Select(p => p.City)
    .Distinct()
    .OrderBy(c => c);
Console.WriteLine("-- Unique cities --\n" + string.Join("\n", uniqueCities));


Console.WriteLine("\nDone. Press ENTER to close...");
Console.ReadLine();

record Person(string Name, int Age, string City, bool LikesCoffee, int HobbiesCount);
