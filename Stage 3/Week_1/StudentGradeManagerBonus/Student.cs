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

    public string LetterGrade
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

    public void CalculateAverageAndGrade()
    {
        //calculate average
        double sum = 0;
        foreach (int score in TestScores)
        {
            sum += score;    
        }
        Average = Math.Round((sum / TestScores.Count), 1);
        
        //determine letter grade
        if (Average >= 90 && Average <= 100)
            LetterGrade = "A";
        else if (Average >= 80)
            LetterGrade = "B";
        else if (Average >= 70)
            LetterGrade = "C";
        else if (Average >= 60)
            LetterGrade = "D";
        else
            LetterGrade = "F";
    }

   
}