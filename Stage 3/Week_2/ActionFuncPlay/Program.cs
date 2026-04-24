using System;
using System.Net;

class Program
{
    static void Main()
    {
        Console.WriteLine("Actions & Funcs playground\n");

        Console.WriteLine("\n=== Task 1 ===");
        SayHello("Valerie");

        Action<string> sayHello = name => Console.WriteLine($"Hello {name}!");
        sayHello("Cooper");


        Console.WriteLine("\n=== Task 2 ===");
        Action<int> showNumber = n => Console.WriteLine($"The number is {n}");
        showNumber(42);


        Console.WriteLine("\n=== Task 3 ===");
        Func<int, int, int> add = (a, b) => a + 6;
        Func<int, int, int> multiply = (a, b) => a * b;
        Console.WriteLine(add(5, 6));
        Console.WriteLine(multiply(4, 9));


        Console.WriteLine("\n=== Task 4 ===");
        DoTwice(() => Console.WriteLine("Hi!"));
        DoTwice(() => Console.Beep()); //need to run from real console to get the beep sound
        DoTwice(() => DoTwice(() => Console.WriteLine("Oh no!!")));


        Console.WriteLine("\n=== Task 5 ===");
        Console.WriteLine(Calculate(10, 3, (x, y) => x + y));
        Console.WriteLine(Calculate(10, 3, (x, y) => x * y));
        Console.WriteLine(Calculate(10, 3, (x, y) => x - y));
        Console.WriteLine(Calculate(10, 3, (x, y) => x / y)); //be aware this is integer division
        

        Console.WriteLine("\n=== Task 6 ===");
        Func<string> getGreeting = () =>
        {
            int hour = DateTime.Now.Hour;
            if (hour < 12) return "Good morning!";
            if (hour < 17) return "Good afternoon!";
            return "Good evening!";
        };
        Console.WriteLine(getGreeting());
        Console.WriteLine(getGreeting());


        Console.WriteLine("\n=== Task 7 ===");
        DoTwiceWithName(n => Console.WriteLine($"Hello {n}!"), "Valerie");

        Console.WriteLine("\nPress ENTER to exit...");
        Console.ReadLine();
    }

    static int Calculate(int a, int b, Func<int, int, int> operation)
    {
        return operation(a ,b);
    }
    static void SayHello(string name)
    {
        Console.WriteLine($"Hello {name}!");
    }

    static void DoTwice (Action action)
    {
        action();
        action();
    }

    // DoTwice with name
    static void DoTwiceWithName(Action<string> action, string name)
    {
        action(name);
        action(name);
    }
}