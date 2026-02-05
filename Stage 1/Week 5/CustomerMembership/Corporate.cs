using System;

namespace CustomerMembership;

class Corporate : Membership
{
    public double CashBackPercent   
        { get; set; }
    
    public Corporate() : base()
    {
        AnnualCost = 75;
        MembershipType = "C";
        CashBackPercent = 1;
    }

    public Corporate(int newID, string newEmail, double newPurchaseAmount) : base(newID, newEmail, newPurchaseAmount)
    {
        AnnualCost = 75;
        MembershipType = "C";
        CashBackPercent = 1;
    }
    public override void CashBackRewards()
    {
        double cashBackAmount = ThisMonthPurchaseAmount * (CashBackPercent / 100);
        ThisMonthPurchaseAmount = 0;
        Console.WriteLine($"Cash-back reward request for membership {MembershipID} in the amount of ${cashBackAmount:F2} has been made.");
    }
    public override string ToString()
    {
        return base.ToString() + $" | Percent cash-back: {CashBackPercent:F2}%";
    }
} //end class

