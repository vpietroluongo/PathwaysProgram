using System;

namespace BankAccounts;

//class Savings : Account, ICalculateInterest
class Savings : InterestEarning
{
    // public double AnnualInterestRate
    //     { get; set;}

    public Savings() : base()
    {
        
    }


    public Savings(int newAccount, double newBalance, string newType, double newInterestRate) : base(newAccount, newBalance, newType, newInterestRate)
    {
        //AnnualInterestRate = newInterestRate;
    }

    // public double CalculateInterest()
    // {
    //     double interestEarned = AccountBalance + (AccountBalance * (AnnualInterestRate / 100));
    //     return interestEarned;
    // }
    public override void CalculateWithdrawal(double withdrawalAmount)
    {
        if (withdrawalAmount > 0)
        {
            if (AccountBalance > withdrawalAmount)
                AccountBalance = AccountBalance - withdrawalAmount;
            else    
                Console.WriteLine("Insufficient funds.");
        } 
        else
            Console.WriteLine("Withdrawal amount must be greater than $0.00.");
        
    }
    // public override string ToString()
    // {
    //     return base.ToString() + $" | Annual Interest Rate: {AnnualInterestRate} | Interest Earned: {CalculateInterest()}";
    // }
} //end class