using System;

namespace BankAccounts;

abstract class InterestEarning : Account, ICalculateInterest
{
    public double AnnualInterestRate
        { get; set; }


    public InterestEarning() : base()
    {
        AnnualInterestRate = 0.00;
    } 

    public InterestEarning(int newAccount, double newBalance, string newType, double newInterestRate) : base(newAccount, newBalance, newType)
    {
        AnnualInterestRate = newInterestRate;
    }

    public double CalculateInterest()
    {
        double interestEarned = AccountBalance * (AnnualInterestRate / 100);
        return interestEarned;
    }

    public override string ToString()
    {
        return base.ToString() + $" | Annual Interest Rate: {AnnualInterestRate}% | Interest Earned: {CalculateInterest():F2}";
    }
}  //end class