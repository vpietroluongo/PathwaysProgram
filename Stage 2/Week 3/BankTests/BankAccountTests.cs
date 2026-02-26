using BankAccountNS;

namespace BankTests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void Debit_WithValidAmount_UpdatesBalance()
        {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = 4.55;
            double expected = 7.44;
            BankAccount account = new BankAccount("Mr Bryan Walton", beginningBalance);

            // Act
            account.Debit(debitAmount);

            // Assert
            double actual = account.Balance;
            Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");
        }

        [TestMethod]
        public void Debit_WhenAmountIsLessThanZero_ShouldThrowArgumentOutOfRange()
        {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = -100.00;
            BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act and Assert
            //Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => account.Debit(debitAmount));  //Assert.ThrowsException no longer supported

            //Before refactor
            //Assert.ThrowsExactly<System.ArgumentOutOfRangeException>(() => account.Debit(debitAmount));

            //After refactor
            // Act
            try
            {
                account.Debit(debitAmount);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                // Assert
                StringAssert.Contains(e.Message, BankAccount.DebitAmountLessThanZeroMessage);
            }
        }

        [TestMethod]
        public void Debit_WhenAmountIsMoreThanBalance_ShouldThrowArgumentOutOfRange()
        {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = 12.00;
            BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act Assert befor refactor
            //Assert.ThrowsExactly<System.ArgumentOutOfRangeException>(() => account.Debit(debitAmount));

            // Act after refactor
            try
            {
                account.Debit(debitAmount);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                //Assert after refactor
                StringAssert.Contains(e.Message, BankAccount.DebitAmountExceedsBalanceMessage);
                return;
            }
            Assert.Fail("The expected exception was not thrown");
        }
    }
}
