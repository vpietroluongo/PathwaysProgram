using System;

namespace BankAccountsDictionary;

class CD : InterestEarning
{
    public double EarlyWithdrawalFee
        { get; set; }
    
    public CD() : base()
    {
        EarlyWithdrawalFee = 0;
    }

    public CD(int newAccount, double newBalance, string newType, double newInterestRate, double newFee) : base(newAccount, newBalance, newType, newInterestRate)
    {
        EarlyWithdrawalFee = newFee;
    }
    
    public override void CalculateWithdrawal(double withdrawalAmount)
    {
        double actualWithdrawalAmount = withdrawalAmount + EarlyWithdrawalFee;

        if (AccountBalance > actualWithdrawalAmount)
        {
            if (withdrawalAmount > 0)
                AccountBalance = AccountBalance - actualWithdrawalAmount;   
            else   
                Console.WriteLine("Withdrawal amount must be greater than $0.00."); 
        } 
        else    
            Console.WriteLine("Isufficient funds.");
    }

    public override string ToString()
    {
        return base.ToString() + $" | Early Penalty fee: {EarlyWithdrawalFee:F2}";
    }
}   //end class