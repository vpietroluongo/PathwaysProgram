using System;

namespace BankAccountsDictionary;

class Checking : Account
{
    public double AnnualFee
        { get; set; }
    
    public Checking() : base()
    {
        AnnualFee = 0;
    }

    public Checking(int newAccount, double newBalance, string newType, double newFee) : base(newAccount, newBalance, newType)
    {
        AnnualFee = newFee;
    }
    
    public override void CalculateWithdrawal(double withdrawalAmount)
    {
        double fiftyPercentOfBalance = AccountBalance * 0.5;

        if (withdrawalAmount <= fiftyPercentOfBalance)
        {
            if (withdrawalAmount > 0)
                AccountBalance = AccountBalance - withdrawalAmount;
            else
                Console.WriteLine("Withdrawal amount must be greater than $0.00.");
        }
        else
            Console.WriteLine($"Can only withdrawal up to ${fiftyPercentOfBalance:F2}");
    }

    public override string ToString()
    {
        return base.ToString() + $" | Annual fee: ${AnnualFee:F2}";
    }
} //end class