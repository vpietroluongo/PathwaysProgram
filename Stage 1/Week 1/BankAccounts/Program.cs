using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace BankAccounts;

class Program
{
    //This program creates a list of bank accounts and lets the user view the list of accounts or withdraw or deposit money into a specified account.
    static void Main(string[] args)
    {
        bool quitProgram = false;
        int indexFound = -1;
        int inputAccount;
        List<Account> listOfAccounts = new List<Account>();
        listOfAccounts.Add(new Savings(12345678, 2000.23, "S", 0.39));
        listOfAccounts.Add(new Checking(87654321, 1000.50, "C", 10));
        listOfAccounts.Add(new CD(56789012, 750, "CD", 1.6, 200));
        listOfAccounts.Add(new CD(56789013, 456457, "CD", 0.5, 75));
        listOfAccounts.Add(new Checking(87654322, 7689.89, "C", 160));
        listOfAccounts.Add(new Savings(12345677, 15844.20, "C", 0.1));
        listOfAccounts.Add(new Savings(1, 100, "S", 0.1));
        listOfAccounts.Add(new Checking(2, 100, "C", 10));
        listOfAccounts.Add(new CD(3, 100, "CD", 1.6, 50));

        do
        {
            Console.WriteLine("L - List all accounts");
            Console.WriteLine("D - Deposit to an account");
            Console.WriteLine("W - Withdrawal from an account");
            Console.WriteLine("Q - Quit transaction processing");

            string input = (Console.ReadLine()).ToUpper();

            switch (input)
            {
                case "L":
                    Console.WriteLine("In L area.");
                    foreach (Account account in listOfAccounts)
                        Console.WriteLine(account);
                    break;
                case "D":
                    Console.Write("Please enter an account to deposit to: ");
                    inputAccount = Convert.ToInt32(Console.ReadLine());
                    indexFound = listOfAccounts.FindIndex(a => a.AccountID == inputAccount);

                    if (indexFound != -1)
                    {
                        Console.Write("How much would you like to deposit: ");
                        double depositAmount = Convert.ToDouble(Console.ReadLine());
                        listOfAccounts[indexFound].CalculateDeposit(depositAmount);
                        Console.WriteLine($"Account {listOfAccounts[indexFound].AccountID} new balance is {listOfAccounts[indexFound].AccountBalance:F2}");
                    }
                    else    
                        Console.WriteLine($"Account {inputAccount} not in system.");
                    break;
                case "W":
                    Console.Write("Please enter an account to withdraw from:");
                    inputAccount = Convert.ToInt32(Console.ReadLine());
                    indexFound = listOfAccounts.FindIndex(a => a.AccountID == inputAccount);

                    if (indexFound != -1)
                    {
                        Console.Write("How much would you like to withdrawal: ");
                        double withdrawalAmount = Convert.ToDouble(Console.ReadLine());
                        listOfAccounts[indexFound].CalculateWithdrawal(withdrawalAmount);
                        Console.WriteLine($"Account {listOfAccounts[indexFound].AccountID} new balance is {listOfAccounts[indexFound].AccountBalance:F2}");
                    }
                    else    
                        Console.WriteLine($"Account {inputAccount} not in system.");
                    break;
                case "Q":
                    Console.WriteLine("In Q area.");
                    quitProgram = true;
                    break;
                default:
                    Console.WriteLine("Please enter a valid option"); 
                    break;   
            } //end switch
        } while (!quitProgram);
    }  //end Main method    
}  //end class