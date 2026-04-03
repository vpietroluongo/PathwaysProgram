// See https://aka.ms/new-console-template for more information

using MathConsole;

Console.WriteLine("Enter two numbers for addition");
Console.WriteLine("Enter your first number");
var number1 = Console.ReadLine();
Console.WriteLine("Enter your second number");
var number2 = Console.ReadLine();

var _mathService = new MathService();

var sum = _mathService.Add(Convert.ToInt32(number1), Convert.ToInt32(number2));
Console.WriteLine($"The sum of {number1} and {number2} = {sum}");