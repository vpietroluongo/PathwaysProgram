using System.IO.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class MathServiceTests
{
    private readonly MathService _mathService;

    public MathServiceTests()
    {
        _mathService = new MathService();
    }

    [TestMethod]
    public void Add_ReturnsCorrectSum()
    {
        // Arrange
        int a = 5;
        int b = 7;

        // Act
        int result = _mathService.Add(a, b);

        // Assert
        Assert.AreEqual(12, result);
    }

    [TestMethod]
    public void Subtract_ReturnsCorrectDifference()
    {
        int result = _mathService.Subtract(10, 4);
        Assert.AreEqual(6, result);
    }

    [TestMethod]
    public void Multiply_ReturnsCorrectProduct()
    {
        int result = _mathService.Multiply(6, 7);
        Assert.AreEqual(42, result);
    }

    [TestMethod]
    public void Divide_ReturnsCorrectQuotient()
    {
        double result = _mathService.Divide(20, 5);
        Assert.AreEqual(4.0, result);
    }

    [TestMethod]
    public void Divide_ThrowsException_WhenDividingByZero()
    {
        Assert.ThrowsExactly<DivideByZeroException>(() => _mathService.Divide(10, 0));
    }

    [TestMethod]
    public void IsEven_ReturnsTrue_ForEvenNumbers()
    {
        Assert.IsTrue(_mathService.IsEven(4));
        Assert.IsTrue(_mathService.IsEven(0));
    }

    [TestMethod]
    public void IsEven_ReturnsFalse_ForOddNumbers()
    {
        Assert.IsFalse(_mathService.IsEven(5));
        Assert.IsFalse(_mathService.IsEven(1));
    }

    [TestMethod]
    public void GetMax_ReturnsLargerNumber()
    {
        Assert.AreEqual(10, _mathService.GetMax(10, 7));
        Assert.AreEqual(25, _mathService.GetMax(3, 25));
        Assert.AreEqual(100, _mathService.GetMax(100, 100));
    }

    [TestMethod]
    public void Divide_ReturnsPositiveQuotient_ForNegativeNumbers()
    {
        //Arrange
        int a = -10;
        int b = -5;

        //Act
        var result = _mathService.Divide(a, b);

        //Assert
        Assert.AreEqual(2, result);
    }

    [TestMethod]
    public void Add_ReturnsNegativeSum_ForNegativeNumbers()
    {
        //Arrange
        int a = -10;
        int b = -5;

        //Act
        var result = _mathService.Add(a, b);

        //Assert
        Assert.AreEqual(-15, result);
    }

    [DataTestMethod]
    [DataRow(0, 0, 0)]
    [DataRow(-5, -7, -12)]
    [DataRow(5, 7, 12)]
    [DataRow(-5, 7, 2)]
    [DataRow(5, -7, -2)]
    public void Add_ReturnsCorrectSum_ForAllScenarios(int a, int b, int expected)
    {
        //Act
        var result = _mathService.Add(a, b);

        //Assert
        Assert.AreEqual(result, expected);
    }
}