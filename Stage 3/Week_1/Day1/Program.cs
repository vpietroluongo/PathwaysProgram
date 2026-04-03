// See https://aka.ms/new-console-template for more information
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Day1;

// Console.Write("Enter your name: ");
// //string name = Console.ReadLine();
// string name = Console.ReadLine();

// if (name.Contains("."))
// {
//     Console.WriteLine("error");
// }
// else
// {
//     Console.WriteLine($"Hello, {name.ToUpper()}!");
//     Console.WriteLine($"Hello, {name.ToLower()}!");
//     Console.WriteLine($"Hello, {name.Trim()}!");
// }
//come back to substring
class Program
{
    static void Main(string[] args)
    {
        string yesNo = "yes";

        while (yesNo == "yes")
        {
            //int number1String = Convert.ToInt32(Console.ReadLine());
            //string number2 = Console.ReadLine();
            //int number2 = int.Parse(Console.ReadLine());
            Console.Write("Enter a number: ");
            string number1String = Console.ReadLine();
            int.TryParse(number1String, out int number1);

            Console.Write("Enter another number: ");
            string number2String = Console.ReadLine();
            int.TryParse(number2String, out int number2);

            Console.Write("Enter operation (add, sub, mul ,div): ");
            string operation = Console.ReadLine().ToLower();
            Calculator calculator = new Calculator(number1, number2);
            
            switch (operation)
            {
                case "add":
                    Console.WriteLine($"{number1} + {number2} = {number1 + number2}");
                    Console.WriteLine($"Individual Sum of {number1} and {number2} using Sum method = {Sum(number1, number2)}");
                    for (int counter = 0; counter < number2; counter++)
                    {
                        Console.WriteLine($"counter = {counter}");
                    }
                    Console.WriteLine($"Sum using calculator.Add() = {calculator.Add()}");
                    break;;
                case "sub":
                    Console.WriteLine($"{number1} - {number2} = {number1 - number2}");
                    Console.WriteLine($"Using calculator.Subtract() = {calculator.Subtract()}");
                    break;
                case "mul":
                    Console.WriteLine($"{number1} * {number2} = {number1 * number2}");
                    Console.WriteLine($"Using calculator.Multiply() = {calculator.Multiply()}");
                    break;
                case "div":
                    if (number2 == 0)
                    {
                        Console.WriteLine("Cannot divide by 0");
                    }
                    else
                    {
                        Console.WriteLine($"{number1} / {number2} = {(double)number1 / number2}");
                    }
                    Console.WriteLine($"Using calculator.Divide() = {calculator.Divide()}");
                    break;
                default:
                    Console.WriteLine("Invalid operation");
                    break;
            }

            Console.Write("Do you want to continue? (yes/no): ");
            yesNo = Console.ReadLine().ToLower();
        }

        Console.Write("Enter your height in inches: ");
        var height = Console.ReadLine();
        Console.WriteLine("You are " + height + " inches tall.");
        int heightInInches = 0;
        while (!int.TryParse(height, out heightInInches))
        {
            Console.Write("Invalid height, Please enter height in inches: ");
            height = Console.ReadLine();
        }
        if (heightInInches > 84)
            Console.WriteLine("You are taller than 7 foot");
        else if (heightInInches > 72)
            Console.WriteLine("You are taller than 6 feet");
        else if (heightInInches > 60)
            Console.WriteLine("You are over 5 feet tall");
        else if (heightInInches == 60)
            Console.WriteLine("You are 5 feet tall");
        else
            Console.WriteLine("You are under 5 feet tall");

        //var hasHeight = height ? true : false;
        //var list = new System.Collections.ArrayList();  //every item won't have a type, so every time want to add or remove something, have to cast it
        List<int> list = new List<int>();
        Stack<int> myStack = new Stack<int>();
        Queue<int> myQueue = new Queue<int>();
        string input = "";
        while(true)
        {
            Console.WriteLine("Enter a number:");
            input = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(input))
                break;
            int number = int.Parse(input);
            list.Add(number);
            myStack.Push(number);
            myQueue.Enqueue(number);
        }

        int sumList = 0;
        int sumStack = 0;
        int sumQueue = 0;
        foreach(int number in list)
        {
            Console.WriteLine(number); 
            sumList += number;  
        }

        sumList = 0;
        sumStack = 0;
        sumQueue = 0;
        for(int i = 0; i <list.Count; i++)
        {
            sumList += list[i];
        }

        sumList = 0;
        sumStack = 0;
        sumQueue = 0;
        int i2 = 0;
        while (i2 < list.Count)
        {
                sumList += list[i2];
                i2++;
        }

        Console.WriteLine($"The list sum of the numbers is {sumList}");



        sumStack = 0;
        foreach (int number in myStack)
        {
            sumStack += number;
        }

        Console.WriteLine($"Foreach stack: The sum is {sumStack}");

        sumStack = 0;
        while (myStack.Count > 0)
        {
            sumStack += myStack.Pop();
        }
        Console.WriteLine($"While stack: The sum if {sumStack}");

        sumQueue = 0;
        while (myQueue.Count > 0)
        {
            sumQueue += myQueue.Dequeue();
        }
        Console.WriteLine($"While queue: The sum is {sumQueue}");

        Console.WriteLine("Enter how many numbers you will enter: ");
        input = Console.ReadLine();
        int arraySize = 0;
        if (!int.TryParse(input, out arraySize))
        {
            Console.WriteLine("Invalid input, using default of 2");
            arraySize = 2;
        }
        else
        {
            arraySize = int.Parse(input);
        }
        int[] array = new int[arraySize];
        for (int i = 0; i < array.Length; i++)
        {
            Console.WriteLine("Enter a number: ");
            string numberInput = Console.ReadLine();
            int number = 0;
            while (!int.TryParse(numberInput, out number))
            {
                Console.WriteLine("Invalid input, please enter a number: ");
                numberInput = Console.ReadLine();
            }
            array[i] = number;
        }

        int sumArray = 0;
        foreach (int number in array)
        {
            sumArray += number;
        }
        Console.WriteLine($"Foreach array: The sum is {sumArray}");

        sumArray = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sumArray += array[i];
        }
        Console.WriteLine($"For array: The sum is {sumArray}");

        sumArray = 0;
        i2 = 0;
        while (i2 < array.Length)
        {
            sumArray += array[i2];
            i2++;
        }
        Console.WriteLine($"While array: The sum is {sumArray}");

        Console.WriteLine($"Array Sum using Sum method = {Sum(array)}");
        Console.WriteLine($"List Sum using Sum method = {Sum(list)}");
        SayHello();

        list.Sort();
        int max = int.MinValue;
        foreach (int number in list)
        {
            if (number > max)
            {
                max = number;
            }
        }
        Console.WriteLine($"The maximum number in the list is {max}");  


        string filename = "myfile.txt";
        
    }
    static int Sum(int number1, int number2)
    {
        return number1 + number2;
    }

    static int Sum(int[] numbers)
    {
        int sum = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            sum += numbers[i];
        }
        return sum;
    }

    static int Sum(List<int> numbers)
    {
        int sum = 0;
        for (int i = 0; i < numbers.Count; i++)
        {
            sum += numbers[i];
        }
        return sum;
    }

    static void SayHello()
    {
        Console.WriteLine("Hello from SayHello method");
    }
}


