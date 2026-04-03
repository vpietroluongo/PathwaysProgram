using System;

namespace StudentGradeManager;

public class Student
{
    public double Average
    { get; set; }
    public string Name
    { get; set; }

    public List<int> TestScores
    { get; set; }


    public Student()
    {
        Name = "";
        TestScores = new List<int>(3);
    }

    public Student(string name, List<int> testScores)
    {
        Name = name;
        TestScores = testScores;
    }  
}