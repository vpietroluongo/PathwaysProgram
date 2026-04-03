using System;
using System.Collections.Generic;
using System.Xml.Schema;

Console.WriteLine("Generics Practice Started\n");

// Your code for the 20 activities goes here

//Section 1: Generic List<T>
Console.WriteLine("\n===Activty 1 ===");
List<int> numbers = new List<int>{10, 20, 30};
numbers.Add(40);
numbers.Add(50);

foreach (int number in numbers)
{
    Console.WriteLine(number);
}


Console.WriteLine("\n===Activty 2 ===");
List<string> names = new List<string>{"Chad", "Cooper", "Dominic", "Jim", "Val"};
Console.WriteLine($"There are {names.Count} names in the list");


Console.WriteLine("\n===Activty 3 ===");
List<double> prices = new List<double>();
prices.Add(5.99);
prices.Add(12.50);
prices.Add(3.75);
prices.RemoveAt(0);

foreach (double price in prices)
{
    Console.WriteLine($"${price:f2}");
}


Console.WriteLine("\n===Activty 4 ===");
List<bool> flags = new List<bool>{true, false, true};

string containsFalse = flags.Contains(false) ? "does" : "does not";
Console.WriteLine($"The flags list {containsFalse} contain false");


Console.WriteLine("\n===Activty 5 ===");
List<int> scores = new List<int>{85, 92, 78};

scores.Sort();
foreach (int score in scores)
{
    Console.WriteLine(score);
}



//Section 2: Activities with Generic Stack<T>
Console.WriteLine("\n===Activty 6 ===");
Stack<int> numbersStack = new Stack<int>();
numbersStack.Push(100);
numbersStack.Push(200);
numbersStack.Push(300);

int top = numbersStack.Pop();
Console.WriteLine($"Popped number is {top}");


Console.WriteLine("\n===Activty 7 ===");
Stack<string> undoStack = new Stack<string>();
undoStack.Push("save");
undoStack.Push("edit");
undoStack.Push("delete");

string topUndoStack = undoStack.Peek();
Console.WriteLine($"The top of the stack is {topUndoStack}");


Console.WriteLine("\n===Activty 8 ===");
Stack<char> letters = new Stack<char>();
letters.Push('A');
letters.Push('B');
letters.Push('C');
letters.Push('D');
while (letters.Count > 0)
{
    char poppedLetter = letters.Pop();
    Console.WriteLine($"The top of the stack is {poppedLetter}");
}


Console.WriteLine("\n===Activty 9 ===");
Stack<int> tempStack = new Stack<int>();
tempStack.Push(10);
tempStack.Push(20);
tempStack.Push(30);
tempStack.Push(40);
tempStack.Push(50);
Console.WriteLine($"There are {tempStack.Count} numbers in the stack");


Console.WriteLine("\n===Activty 10 ===");
Stack<string> browserHistory = new Stack<string>();
browserHistory.Push("www.google.com");
browserHistory.Push("www.yahoo.com");
browserHistory.Push("www.gmail.com");
string lastWebsite = browserHistory.Pop();
Console.WriteLine($"Navigated back from {lastWebsite}");


//Section 3: Activities with generic Queue<T>
Console.WriteLine("\n===Activty 11 ===");
Queue<string> orderQueue = new Queue<string>();
orderQueue.Enqueue("Bob Smith");
orderQueue.Enqueue("Alice Jones");
orderQueue.Enqueue("Joe Miller");
string first = orderQueue.Dequeue();
Console.WriteLine($"The first order is {first}");


Console.WriteLine("\n===Activty 12 ===");
Queue<int> ticketQueue = new Queue<int>();
for (int i = 101; i <= 105; i++)
{
    ticketQueue.Enqueue(i);
}
int firstInLine = ticketQueue.Peek();
Console.WriteLine($"The first ticket is {firstInLine}");


Console.WriteLine("\n===Activty 13 ===");
Queue<string> tasks = new Queue<string>();
tasks.Enqueue("Task1");
tasks.Enqueue("Task2");
tasks.Enqueue("Task3");
while (tasks.Count > 0)
{
    Console.WriteLine($"Performing {tasks.Dequeue()}");
}


Console.WriteLine("\n===Activty 14 ===");
Queue<bool> flagsQueue = new Queue<bool>();
flagsQueue.Enqueue(true);
flagsQueue.Enqueue(false);
flagsQueue.Enqueue(true);
Console.WriteLine($"There are {flagsQueue.Count} flags, and the first one is {flagsQueue.Peek()}");


Console.WriteLine("\n===Activty 15 ===");
Queue<double> payments = new Queue<double>();
payments.Enqueue(50.34);
payments.Enqueue(75.50);
payments.Enqueue(100.99);
double paid = payments.Dequeue();
Console.WriteLine($"Processed payment of {paid}");


//Section 4: Activities with Custom Generic Type Message<T>
Console.WriteLine("\n===Activty 16 ===");
Message<string> myMessage = new Message<string>("Hello world!");
Console.WriteLine(myMessage.Content);


Console.WriteLine("\n===Activty 17 ===");
Message<int> numericMessage = new Message<int>(42);
Console.WriteLine(numericMessage.Content);


Console.WriteLine("\n===Activty 18 ===");
Message<bool> statusMessage = new Message<bool>(true);
Console.WriteLine($"Status: {statusMessage.Content}");


Console.WriteLine("\n===Activty 19 ===");
Message<double> priceMessage = new Message<double>(99.99);
Console.WriteLine($"Price message: ${priceMessage.Content}");


Console.WriteLine("\n===Activty 20 ===");
List<Message<string>> listOfMessages = new List<Message<string>>();
listOfMessages.Add(new Message<string>("message 1"));
listOfMessages.Add(new Message<string>("message 2"));
foreach (Message<string>message in listOfMessages)
{
    Console.WriteLine(message.Content);
}


Console.WriteLine("\nAll activities completed. Press ENTER to exit...");
Console.ReadLine();


class Message<T>
{
    public T Content { get; set; }
    public Message(T content)
    {
        Content = content;
    }
}