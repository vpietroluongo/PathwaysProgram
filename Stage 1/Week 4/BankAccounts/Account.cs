using System;

namespace BankAccounts;

abstract class Account
{
    public int AccountID
        { get; set; }
    
    public double AccountBalance
        { get; set; }
    
    public string AccountType
        { get; set; }
    
    public Account()
    {
        AccountID = 0;
        AccountBalance = 0.00;
        AccountType = "";
    }

    public Account(int newAccount, double newBalance, string newType)
    {
        AccountID = newAccount;
        AccountBalance = newBalance;
        AccountType = newType;
    }

    public void CalculateDeposit(double depositAmount)
    {
        if (depositAmount > 0)
        {
            AccountBalance = AccountBalance + depositAmount;
        }
        else
            Console.WriteLine("Deposit must be greater than $0.00");
    }

    public abstract void CalculateWithdrawal(double withdrawalAmount);

    public override string ToString()
    {
        return $"Account ID: {AccountID} | Account Type: {AccountType} | Balance: ${AccountBalance:F2}";
    }
}  //end class