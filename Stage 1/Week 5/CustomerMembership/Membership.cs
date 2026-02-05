using System;

namespace CustomerMembership;

abstract class Membership
{
    public double AnnualCost
        { get; set; }
    public int MembershipID
        { get; set; }
    public string MembershipType
        { get; set; }
    public string PrimaryEmail
        { get; set; }
    public double ThisMonthPurchaseAmount
        { get; set; }
    
    public Membership()
    {
        AnnualCost = 0;
        MembershipID = 0;
        MembershipType = "";
        PrimaryEmail = "";
        ThisMonthPurchaseAmount = 0; 
    }

    //public Membership(double newCost, int newID, string newType, string newEmail, double newPurchaseAmount)
    public Membership(int newID, string newEmail, double newPurchaseAmount)
    {
        MembershipID = newID;
        PrimaryEmail = newEmail;
        ThisMonthPurchaseAmount = newPurchaseAmount;
    }

    public abstract void CashBackRewards();

    public void Purchase(double purchaseAmount)
    {
        if (purchaseAmount > 0)
            ThisMonthPurchaseAmount += purchaseAmount;   
        else    
            Console.WriteLine("Purchase must be greater than $0."); 
    }

    public void Return(double returnAmount)
    {
        if (returnAmount <= ThisMonthPurchaseAmount)
        {
            if (returnAmount > 0)
                ThisMonthPurchaseAmount -= returnAmount;   
            else
                Console.WriteLine("Return must be great than $0.");
        }
        else
            Console.WriteLine($"Return amount can only be up to ${ThisMonthPurchaseAmount:F2}.");
    }

    public override string ToString()
    {
        return $"Membership ID: {MembershipID} | Membership type: {MembershipType} | Annual Cost: ${AnnualCost:F2} | Primary Email: {PrimaryEmail} | Purchase Amount: ${ThisMonthPurchaseAmount:F2}";   
    }
}  //end class