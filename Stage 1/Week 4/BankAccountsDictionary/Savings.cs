using System;

namespace BankAccountsDictionary;

class Savings : InterestEarning
{
    public Savings() : base()
    {}

    public Savings(int newAccount, double newBalance, string newType, double newInterestRate) : base(newAccount, newBalance, newType, newInterestRate)
    {}

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
} //end class