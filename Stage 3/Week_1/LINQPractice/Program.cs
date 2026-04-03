using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


var people = new List<Person>
        {
            new("Mia",     24, "Berlin",     true,  new List<string>{ "skiing", "gaming", "coffee" }),
            new("Lucas",   31, "Lisbon",     false, new List<string>{ "surfing", "guitar" }),
            new("Aisha",   19, "Toronto",    true,  new List<string>{ "photography", "hiking", "gaming" }),
            new("Noah",    42, "Austin",     true,  new List<string>{ "bbq", "motorcycles", "coffee", "gaming" }),
            new("Sofia",   28, "Toronto",  false, new List<string>{ "dancing", "yoga" }),
            new("Kai",     22, "Seoul",      true,  new List<string>{ "gaming", "street food", "photography" })
        };

Console.WriteLine("Data ready. Start playing with LINQ!\n");

Console.WriteLine("young");
var young = people.Where(p => p.Age < 30);
foreach (var person in young)
{
    Console.WriteLine(person.Name);
}
Console.WriteLine("---------------------");



Console.WriteLine("coffeeLovers");
var coffeeLovers = people
    .Where(p => p.LikesCoffee);
foreach (var person in coffeeLovers)
{
    Console.WriteLine(person.Name);
}
Console.WriteLine("---------------------");



Console.WriteLine("bCityPeople");
var bCityPeople = people
    .Where(p => p.City.StartsWith("B"));
foreach (var person in bCityPeople)
{
    Console.WriteLine(person.Name);
}
Console.WriteLine("---------------------");



Console.WriteLine("under25");
var under25 = people
    .Where(p => p.Age < 25)
    .OrderBy(p => p.Age);
foreach (var person in under25)
{
    Console.WriteLine(person.Name);
}
Console.WriteLine("---------------------");



Console.WriteLine("hobbies");
var hobbies = people
    .SelectMany(p => p.Hobbies);
foreach (var hobby in hobbies)
{
    Console.WriteLine(hobby);
}
Console.WriteLine("---------------------");



Console.WriteLine("coffeeAndGamingLovers");
var coffeeAndGamingLovers = people
    .Where(p => p.LikesCoffee)
    .Where(p => p.Hobbies.Contains("gaming"));
foreach (var person in coffeeAndGamingLovers)
{
    Console.WriteLine(person.Name);
}
Console.WriteLine("---------------------");



Console.WriteLine("hasMostHobbies");
var hasMostHobbies = people
    .OrderByDescending(p => p.Hobbies.Count)
    .First();  //wouldn't work right if more than one person has the most hobbies
Console.WriteLine(hasMostHobbies.Name);
Console.WriteLine("---------------------");



Console.WriteLine("coffeeVsNot");
var coffeeVsNot = people
    .GroupBy(p => p.LikesCoffee)
    .Select(g => new
    {
        LikesCoffee = g.Key,
        Count = g.Count()
    });
foreach (var group in coffeeVsNot)
{
    Console.WriteLine(group);
}
Console.WriteLine("---------------------");


Console.WriteLine("citiesWithMin2");
var citiesWithMin2 = people
    .GroupBy(p => p.City)
    .Where(g => g.Count() >= 2)
    .Select(g => new
    {
        City = g.Key,
        Count = g.Count()
    });
foreach (var city in citiesWithMin2)
{
    Console.WriteLine(city);
}
Console.WriteLine("---------------------");

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
    public bool LikesCoffee { get; set; }
    public List<string> Hobbies { get; set; } = new List<string>();

    public Person(string name, int age, string city, bool likesCoffee, List<string> hobbies)
    {
        Name = name;
        Age = age;
        City = city;
        LikesCoffee = likesCoffee;
        Hobbies = hobbies;
    }
}