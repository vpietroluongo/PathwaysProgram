using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace BankAccountsDictionary;

class Program
{
    //This program creates a dictionary of bank accounts and lets the user view the info for all of the accounts or withdraw or deposit money into a specified account.
    static void Main(string[] args)
    {
        bool quitProgram = false;
        int inputAccount;

        Dictionary<int, Account> dictionaryOfAccounts= new Dictionary<int, Account>();
        dictionaryOfAccounts.Add(12345678, new Savings(12345678, 2000.23, "S", 0.39));
        dictionaryOfAccounts.Add(87654321, new Checking(87654321, 1000.50, "C", 10));
        dictionaryOfAccounts.Add(56789012, new CD(56789012, 750, "CD", 1.6, 200));
        dictionaryOfAccounts.Add(56789013, new CD(56789013, 456457, "CD", 0.5, 75));
        dictionaryOfAccounts.Add(87654322, new Checking(87654322, 7689.89, "C", 160));
        dictionaryOfAccounts.Add(12345677, new Savings(12345677, 15844.20, "C", 0.1));
        dictionaryOfAccounts.Add(1, new Savings(1, 100, "S", 0.1));
        dictionaryOfAccounts.Add(2, new Checking(2, 100, "C", 10));
        dictionaryOfAccounts.Add(3, new CD(3, 100, "CD", 1.6, 50));

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
                    foreach (Account account in dictionaryOfAccounts.Values)
                        Console.WriteLine(account);
                    break;
                case "D":
                    Console.Write("Please enter an account to deposit to: ");
                    inputAccount = Convert.ToInt32(Console.ReadLine());

                    if (dictionaryOfAccounts.ContainsKey(inputAccount))
                    {
                        Console.Write("How much would you like to deposit: ");
                        double depositAmount = Convert.ToDouble(Console.ReadLine());
                        dictionaryOfAccounts[inputAccount].CalculateDeposit(depositAmount);
                        Console.WriteLine($"Account {dictionaryOfAccounts[inputAccount].AccountID} new balance is {dictionaryOfAccounts[inputAccount].AccountBalance:F2}");
                    }
                    else    
                        Console.WriteLine($"Account {inputAccount} not in system.");
                    break;
                case "W":
                    Console.Write("Please enter an account to withdraw from:");
                    inputAccount = Convert.ToInt32(Console.ReadLine());

                    if (dictionaryOfAccounts.ContainsKey(inputAccount))
                    {
                        Console.Write("How much would you like to withdrawal: ");
                        double withdrawalAmount = Convert.ToDouble(Console.ReadLine());
                        dictionaryOfAccounts[inputAccount].CalculateWithdrawal(withdrawalAmount);
                        Console.WriteLine($"Account {dictionaryOfAccounts[inputAccount].AccountID} new balance is {dictionaryOfAccounts[inputAccount].AccountBalance:F2}");
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