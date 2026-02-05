using System;
using System.Diagnostics.Tracing;
using System.Runtime.Intrinsics.Arm;

namespace CustomerMembership;
class Program
{
    //This program creates a list of customer memberships and lets the user view the info for all of the memberships or create, delete or updated a membership
            /*  prompt user and obtain option 
            C section
                prompt user for new ID and and obtain value
                search for index of that id in the list
                if index found
                    output already in system message
                else
                    prompt user and obtain rest of new values
                    add newly instantiated membership to list
            R section
                for each membership in the list
                    write out the membership ID
            U section
                prompt user and obtain account id
                search for index of that id in the list
                if index found
                    prompt user and obtain option to update
                    update that option
                else
                    membership not found
            D section
                prompt user and obtain account id
                search for index of that id in the list
                if index found
                    remove the membership at that index
                else
                    membership not found
            L section
                for each membership in the list
                    write out all the information for that membership
            P or T sections
                prompt user and obtain account id
                search for index of that id in the list
                if index found
                    prompt user for purchase/return amount
                    invoke purchase/return method
                else
                    membership not found
            A
                prompt user and obtain account id
                search for index of that id in the list
                if index found
                    invoke calculate cash-back method
                else
                    membership not found
            Q section
                change quit program flag to true
        */
    static void Main(string[] args)
    {
        bool quitProgram = false;
        int inputID;
        int foundID = -1;
        string newType;
        string newEmail;
        string newNonProfitType;
        List<Membership> listOfMemberships = new List<Membership>();
        listOfMemberships.Add(new Regular(12345678, "name@gmail.com", 0));
        listOfMemberships.Add(new Executive(23456789, "email2@yahoo.com", 1001));
        listOfMemberships.Add(new Executive(34567890, "email2@yahoo.com", 999));
        listOfMemberships.Add(new NonProfit(45678901, "email3@hotmail.com", 100, "M"));
        listOfMemberships.Add(new NonProfit(56789012, "email3@hotmail.com", 100, "E"));
        listOfMemberships.Add(new NonProfit(67890123, "email@gmail.com", 100, "O"));
        listOfMemberships.Add(new Corporate(78901234, "name5@gmail.com", 0));

        //loop until Q is entered and quitProgram flag changes 
        do
        {
            Console.WriteLine("Administative Options:");
            Console.WriteLine("C - Create a new membership");
            Console.WriteLine("R - Read all memberships");
            Console.WriteLine("U - Update an existing membership");
            Console.WriteLine("D - Delete an existing membership");
            Console.WriteLine("----------------------");
            Console.WriteLine("Transaction Options:");
            Console.WriteLine("L - List all memberships and their info");
            Console.WriteLine("P - Purchase transaction");
            Console.WriteLine("T - Return transaction");
            Console.WriteLine("A - Apply cash-bak rewards");
            Console.WriteLine("Q - Quit program");

            string input = (Console.ReadLine()).ToUpper();

            switch (input)
            {
                case "C":
                    Console.Write("Please enter new membership ID: ");
                    int newID = Convert.ToInt32(Console.ReadLine());
                    if (listOfMemberships.Exists(m => m.MembershipID == newID))   //check if ID input by user already exists
                        Console.WriteLine($"{newID} already in system");
                    else
                    {
                        Console.Write("Please enter new email: ");
                        newEmail = Console.ReadLine();
                        Console.Write("Please enter new type: ");
                        newType = (Console.ReadLine()).ToUpper();
                        Console.Write("Please enter a purchase amount: ");
                        double newPurchaseAmount = Convert.ToDouble(Console.ReadLine());

                        switch (newType)
                        {
                            case "R":
                                listOfMemberships.Add(new Regular(newID, newEmail, newPurchaseAmount));
                                // Console.Write("Does this membership get a special offer? Y or N: ");
                                // input = (Console.ReadLine()).ToUpper();
                                // if (input == "Y")
                                //     ((Regular)listOfMemberships.Last()).CalculateSpecialOffer();  //invoke special offer for last element in list
                                break;
                            case "E":
                                listOfMemberships.Add(new Executive(newID, newEmail, newPurchaseAmount));
                                // Console.Write("Does this membership get a special offer? Y or N: ");
                                // input = (Console.ReadLine()).ToUpper();
                                // if (input == "Y")
                                //     ((Executive)listOfMemberships.Last()).CalculateSpecialOffer();  //invoke special offer for last element in list
                                break;
                            case "N":
                                Console.Write("Please enter the type of non-profit (M, E, O, etc): ");
                                newNonProfitType = (Console.ReadLine()).ToUpper();
                                listOfMemberships.Add(new NonProfit(newID, newEmail, newPurchaseAmount, newNonProfitType));
                                break;
                            case "C":
                                listOfMemberships.Add(new Corporate(newID, newEmail, newPurchaseAmount));
                                break;
                            default:
                                Console.WriteLine("Invalid type");
                                break;
                        } //end switch
                    }   

                    break;
                case "R":
                    foreach (Membership account in listOfMemberships)
                        Console.WriteLine(account.MembershipID);
                    break;
                case "U":
                    Console.Write("Please enter membership ID to update:");
                    inputID = Convert.ToInt32(Console.ReadLine());

                    foundID = listOfMemberships.FindIndex(m => m.MembershipID == inputID); 

                    if (foundID != -1)
                    {
                        Console.Write("Please enter option to update: type, cost, email, cash-back, nonprofit type: ");
                        input = Console.ReadLine().ToLower();

                        switch (input)
                        {
                            case "type":
                                Console.Write("Enter new type (R, E, N, C): ");
                                newType = Console.ReadLine().ToUpper();
                                switch (newType)
                                {
                                    case "R":
                                        Regular newRegular = new Regular(listOfMemberships[foundID].MembershipID, 
                                                                         listOfMemberships[foundID].PrimaryEmail, 
                                                                         listOfMemberships[foundID].ThisMonthPurchaseAmount);
                                        listOfMemberships[foundID] = newRegular;
                                        break;
                                    case "E":
                                        Executive newExecutive = new Executive(listOfMemberships[foundID].MembershipID, 
                                                                               listOfMemberships[foundID].PrimaryEmail, 
                                                                               listOfMemberships[foundID].ThisMonthPurchaseAmount);
                                        listOfMemberships[foundID] = newExecutive;
                                        break;
                                    case "N":
                                        Console.Write("Please enter nonprofit type (M, E, O, etc): ");
                                        newNonProfitType = (Console.ReadLine()).ToUpper();
                                        NonProfit newNonProfit = new NonProfit(listOfMemberships[foundID].MembershipID, 
                                                                               listOfMemberships[foundID].PrimaryEmail, 
                                                                               listOfMemberships[foundID].ThisMonthPurchaseAmount, 
                                                                               newNonProfitType);
                                        listOfMemberships[foundID] = newNonProfit;
                                        break;
                                    case "C":
                                        Corporate newCorporate = new Corporate(listOfMemberships[foundID].MembershipID, 
                                                                               listOfMemberships[foundID].PrimaryEmail, 
                                                                               listOfMemberships[foundID].ThisMonthPurchaseAmount);
                                        listOfMemberships[foundID] = newCorporate;
                                        break;
                                    default:
                                        Console.WriteLine("Invalild type.");
                                        break;
                                } //end switch
                                break;
                            case "cost":
                                Console.Write("Enter new cost: ");
                                double newCost = Convert.ToDouble(Console.ReadLine());
                                listOfMemberships[foundID].AnnualCost = newCost;
                                break;
                            case "email":
                                Console.Write("Enter new email: ");
                                newEmail = Console.ReadLine();
                                listOfMemberships[foundID].PrimaryEmail = newEmail;
                                break;
                            case "cash-back":
                                Console.Write("Enter new percent cash-back: ");
                                double newCashBack = Convert.ToDouble(Console.ReadLine());
                                if (listOfMemberships[foundID] is Regular)
                                    ((Regular) listOfMemberships[foundID]).CashBackPercent = newCashBack;
                                else if (listOfMemberships[foundID] is Executive)
                                {  
                                    Console.Write("Which cash-back reward are you changing? >=1000 or <1000: ");
                                    input = Console.ReadLine();
                                    if (input == ">=1000")
                                        ((Executive) listOfMemberships[foundID]).CashBackPercentAbove1000 = newCashBack;
                                    else if (input == "<1000")
                                        ((Executive) listOfMemberships[foundID]).CashBackPercentBelow1000 = newCashBack;
                                    else    
                                        Console.WriteLine("Invalid option.");
                                }
                                else if (listOfMemberships[foundID] is NonProfit)
                                    ((NonProfit) listOfMemberships[foundID]).CashBackPercent = newCashBack;
                                else if (listOfMemberships[foundID] is Corporate)
                                    ((Corporate) listOfMemberships[foundID]).CashBackPercent = newCashBack;
                                break;
                            case "nonprofit type":
                                if (listOfMemberships[foundID] is NonProfit)
                                {
                                    Console.Write("Enter new non-profit type (M, E, O, etc): ");
                                    newNonProfitType = (Console.ReadLine()).ToUpper();
                                    ((NonProfit) listOfMemberships[foundID]).NonProfitType = newNonProfitType;
                                }
                                else    
                                    Console.WriteLine($"Membership {listOfMemberships[foundID].MembershipID} is not non-profit.");
                                break;
                            default:
                                break;
                        }   //end switch 
                    }
                    else
                        Console.WriteLine($"ID {inputID} not found in system.");

                    break;
                case "D":
                    Console.Write("Please enter membership ID to delete: ");
                    inputID = Convert.ToInt32(Console.ReadLine());
                    foundID = listOfMemberships.FindIndex(m => m.MembershipID == inputID);
                         
                    if (foundID != -1)
                        listOfMemberships.RemoveAt(foundID);
                    else    
                        Console.WriteLine($"ID {inputID} not found in system.");

                    break;
                case "L":
                    foreach (Membership account in listOfMemberships)
                        Console.WriteLine(account);
                    break;
                case "P":
                    Console.Write("Enter membership ID purchase is for: ");
                    inputID = Convert.ToInt32(Console.ReadLine());

                    foundID = listOfMemberships.FindIndex(m => m.MembershipID == inputID);

                    if (foundID != -1)
                    {
                        Console.Write("Enter purchase amount: ");
                        double inputPurchase = Convert.ToDouble(Console.ReadLine());
                        listOfMemberships[foundID].Purchase(inputPurchase);
                    }
                    else
                        Console.WriteLine($"ID {inputID} not found in system.");
                    break;
                case "T":
                    Console.Write("Enter membership ID return is for: ");
                    inputID = Convert.ToInt32(Console.ReadLine());

                    foundID = listOfMemberships.FindIndex(m => m.MembershipID == inputID);

                    if (foundID != -1)
                    {
                        Console.Write("Enter return amount: ");
                        double inputReturn = Convert.ToDouble(Console.ReadLine());
                        listOfMemberships[foundID].Return(inputReturn);
                    }
                    else
                        Console.WriteLine($"ID {inputID} not found in system.");
                    break;
                case "A":
                    Console.Write("Enter membership ID cash-back is for: ");
                    inputID = Convert.ToInt32(Console.ReadLine());

                    foundID = listOfMemberships.FindIndex(m => m.MembershipID == inputID);

                    if (foundID != -1)
                        listOfMemberships[foundID].CashBackRewards();
                    else
                        Console.WriteLine($"ID {inputID} not found in system.");
                    break;
                case "Q":
                    quitProgram = true;
                    break;
                default:
                    Console.WriteLine("Please enter a valid option");
                    break;
            } //end switch
        } while (!quitProgram);   
    }  //end Main method
}  //end class