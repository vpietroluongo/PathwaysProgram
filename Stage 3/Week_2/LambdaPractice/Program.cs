using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Transactions;

class Program
{
    static void Main()
    {
        Console.WriteLine("Actions & Funcs playground\n");

        Console.WriteLine("\n=== Task 1 ===");
        Func<int, int> doubleNumber = n => 2 * n;
        Console.WriteLine($"Double number: {doubleNumber(5)}");
        Console.WriteLine($"Double number: {doubleNumber(12)}");
        Console.WriteLine($"Double number: {doubleNumber(100)}");

        Console.WriteLine("\n=== Task 2 ===");
        Action<string> saySomethingNice = name => Console.WriteLine($"You're awesome, {name}!");
        saySomethingNice("Cooper");
        saySomethingNice("Dominic");
        saySomethingNice("Jim");

        Console.WriteLine("\n=== Task 3 ===");
        Func<int, bool> isEven = n => n % 2 == 0;
        Console.WriteLine($"4 is even: {isEven(4)}");
        Console.WriteLine($"7 is even: {isEven(7)}");
        Console.WriteLine($"0 is even: {isEven(0)}");
        Console.WriteLine($"13 is even: {isEven(13)}");

        Console.WriteLine("\n=== Task 4 ===");
        Twice(() => Console.WriteLine("Message goes here"));
        Twice(() => Console.WriteLine(isEven(5)));

        Console.WriteLine("\n=== Task 5 ===");
        Console.WriteLine(Operate(36, 4, (x, y) => x + y));
        Console.WriteLine(Operate(36, 4, (x, y) => x - y));
        Console.WriteLine(Operate(36, 4, (x, y) => x * y));

        Console.WriteLine("\n=== Task 6 ===");
        Func<string> greeting = () =>
        {
            int hour = DateTime.Now.Hour;
            return hour < 12 ? "Morning!" : hour < 17 ? "Afternoon!" : "Evening!";
        };

        Console.WriteLine($"Good {greeting()}");

        Console.WriteLine("\n=== Task 7 ===");
        Func<int, string> bigOrSmall = n =>
        {
            return n > 100 ? "big" : "small";
        };
        Console.WriteLine($"57 is {bigOrSmall(57)}");
        Console.WriteLine($"101 is {bigOrSmall(101)}");

        Console.WriteLine("\nPress ENTER to exit...");
        Console.ReadLine();
    }

    static void Twice(Action action)
    {
        action();
        action();
    }

    static int Operate(int a, int b, Func<int, int, int> op)
    {
        return op(a, b);
    }
}
