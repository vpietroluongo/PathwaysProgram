using MathConsole;

namespace MathTests;

[TestClass]
public sealed class MathTests
{
private readonly MathService _mathService;

    public MathTests()
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
    public void Multiply_ReturnsCorrectProduct()
    {
        int result = _mathService.Multiply(6, 7);
        Assert.AreEqual(42, result);
    }
}
