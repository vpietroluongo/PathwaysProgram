[TestClass]
public class Test1
{
    private OrderValidator _validator;

    [TestInitialize]
    public void Setup()
    {
        _validator = new OrderValidator();
    }

    [TestMethod]
    [DataRow(0.0)]
    [DataRow(-1.0)]
    public void IsValidOrder_OrderWithZeroOrNegativeTotal_ShouldReturnFalse(double totalAmount)
    {
        //Arrange
        int itemCount = 1;
        bool isMember = true;
        bool hasCoupon = true;

        //Act
        var result = _validator.IsValidOrder((decimal)totalAmount, itemCount, isMember, hasCoupon);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValidOrder_OrderWithZeroItems_ShouldReturnFalse()
    {
        //Arrange
        decimal totalAmount = 5.6m;
        int itemCount = 0;
        bool isMember = true;
        bool hasCoupon = true;

        //Act
        var result = _validator.IsValidOrder(totalAmount, itemCount, isMember, hasCoupon);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValidOrder_NotAMemberAndAmountBelow25_ShouldReturnFalse()
    {
        //Arrange
        decimal totalAmount = 24.99m;
        int itemCount = 10;
        bool isMember = false;
        bool hasCoupon = true;

        //Act
        var result = _validator.IsValidOrder(totalAmount, itemCount, isMember, hasCoupon);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValidOrder_NotAMemberAndAmountAbove25_ShouldReturnTrue()
    {
        //Arrange
        decimal totalAmount = 26m;
        int itemCount = 10;
        bool isMember = false;
        bool hasCoupon = true;

        //Act
        var result = _validator.IsValidOrder(totalAmount, itemCount, isMember, hasCoupon);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValidOrder_MemberAndHasCoupon_AmountLessThan15_ShouldReturnFalse()
    {
        //Arrange
        decimal totalAmount = 14.99m;
        int itemCount = 10;
        bool isMember = true;
        bool hasCoupon = true;

        //Act
        var result = _validator.IsValidOrder(totalAmount, itemCount, isMember, hasCoupon);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValidOrder_MemberAndHasCoupon_AmountMoreThan15_ShouldReturnTrue()
    {
        //Arrange
        decimal totalAmount = 16m;
        int itemCount = 10;
        bool isMember = true;
        bool hasCoupon = true;

        //Act
        var result = _validator.IsValidOrder(totalAmount, itemCount, isMember, hasCoupon);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValidOrder_MemberAndNoCoupon_AmountLessThan20_ShouldReturnFalse()
    {
        //Arrange
        decimal totalAmount = 19.99m;
        int itemCount = 10;
        bool isMember = true;
        bool hasCoupon = false;

        //Act
        var result = _validator.IsValidOrder(totalAmount, itemCount, isMember, hasCoupon);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValidOrder_MemberAndNoCoupon_AmountMoreThan20_ShouldReturnTrue()
    {
        //Arrange
        decimal totalAmount = 21m;
        int itemCount = 10;
        bool isMember = true;
        bool hasCoupon = false;

        //Act
        var result = _validator.IsValidOrder(totalAmount, itemCount, isMember, hasCoupon);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValidOrder_MemberWithCoupon_HighAmount_ReturnsTrue()
    {
        bool result = _validator.IsValidOrder(150m, 5, true, true);
        Assert.IsTrue(result);
    }
}
