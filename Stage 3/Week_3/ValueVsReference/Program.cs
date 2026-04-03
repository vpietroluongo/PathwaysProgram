using System;

Console.WriteLine("Value Types vs Reference Types Activity\n");

// TODO: Complete the 10 activities below
Console.WriteLine("\n=== Activity 1 ===");
int a = 1;
int b = a;  //copy is made
b = 300;
Console.WriteLine($"a = {a}");
Console.WriteLine($"b = {b}");
Console.WriteLine("Notice the value of b changed but the value of a stayed the same");


Console.WriteLine("\n=== Activity 2 ===");
var point1 = new Point(10, 20);
var point2 = new Point(3, 5);   
point2 = point1;   //copy of struct
point2.Y = 999;
Console.WriteLine($"point1: X = {point1.X} | Y = {point1.Y}");
Console.WriteLine($"point2: X = {point2.X} | Y = {point2.Y}");


Console.WriteLine("\n=== Activity 3 ===");
var student1 = new Student{Name = "Alice"};
var student2 = new Student{Name = "Bob"};
student2 = student1;
student2.Name = "Frank";
Console.WriteLine($"student1: {student1.Name}");
Console.WriteLine($"student2: {student2.Name}");


Console.WriteLine("\n=== Activity 4 ===");
string word1 = "candy";
string word2 = "pencil";
word2 = word1;
word2 = "factory";
Console.WriteLine($"word1 = {word1}");
Console.WriteLine($"word2 = {word2}");


Console.WriteLine("\n=== Activity 5 ===");
int number = 3;
ModifyInteger(number);
Console.WriteLine($"number after method = {number}");


Console.WriteLine("\n=== Activity 6 ===");
var originalStudent = new Student{Name = "Jane"};
ModifyStudent(originalStudent);
Console.WriteLine($"student name after ModifyStudent = {originalStudent.Name}");


Console.WriteLine("\n=== Activity 7 ===");
int[] array1 = {10, 20, 30};
int[] array2 = {3, 7, 11};
array2 = array1;
array2[2] = 999;
Console.WriteLine($"array1: {array1[0]}, {array1[1]}, {array1[2]}");
Console.WriteLine($"array2: {array2[0]}, {array2[1]}, {array2[2]}");


Console.WriteLine("\n=== Activity 8 ===");
var rectangle1 = new Rectangle{Width = 3, Height = 4};
var rectangle2 = new Rectangle{Width = 7, Height = 10};
rectangle2 = rectangle1;
rectangle2.Height = 35;
Console.WriteLine($"rectangle1: Width = {rectangle1.Width} | Height = {rectangle1.Height}");
Console.WriteLine($"rectangle2: Width = {rectangle2.Width} | Height = {rectangle2.Height}");


Console.WriteLine("\n=== Activity 9 ===");
var car1 = new Car("Ford");
var car2 = new Car("Toyota");
car2 = car1;
car2.Model = "Honda";
Console.WriteLine($"car1: Model = {car1.Model}");
Console.WriteLine($"car2: Model = {car2.Model}");


Console.WriteLine("\n=== Activity 10 ===");
//Explain why modifying a struct does not affect the original variable, but modifying a class does:
//When you assign one struct to another struct, you
//are making a copy of the original struct, and each 
//copy is independent because structs are value types.
//When you assign one class to another class, you are 
//not making a copy of the original class, but rather
//assigning the value of the original's location in 
//memory.  Each variable then is not independent of
//each other, but are pointing to the same object
// because they are reference types.

//Solution provided:
// Value types (like structs and primitives) store the actual data.
// When you assign them, a full copy is created.
// Reference types (like classes) store a reference to the data on the heap.
// When you assign them, only the reference is copied, so both variables point to the same object.

Console.WriteLine("\nActivity completed. Press ENTER to exit...");
Console.ReadLine();

static void ModifyInteger(int number)
{
    number = number * 56;
    Console.WriteLine($"number inside the method = {number}");   
}

static void ModifyStudent(Student student)
{
    student.Name = "Name from inside ModifyStudent";
    Console.WriteLine($"student name inside ModifyStudent = {student.Name}");
}

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Student
{
    public string Name { get; set; }
}

public struct Rectangle
{
    public int Width { get; set; }
    public int Height { get; set; }
}

public class Car
{
    public string Model { get; set; }

    public Car(string model)
    {
        Model = model;
    }
}