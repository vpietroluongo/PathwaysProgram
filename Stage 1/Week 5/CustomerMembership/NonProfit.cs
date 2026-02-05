using System;

namespace CustomerMembership;

class NonProfit : Membership
{
    public double CashBackPercent
        { get; set; }
    public string NonProfitType
        { get; set;}
    
    public NonProfit() : base()
    {
        AnnualCost = 55;
        CashBackPercent = 2;
        MembershipType = "N";
        NonProfitType = "O";
    }

    public NonProfit(int newID, string newEmail, double newPurchaseAmount, string newNonprofitType) : base(newID, newEmail, newPurchaseAmount)
    {
        AnnualCost = 55;
        CashBackPercent = 2;
        MembershipType = "N";
        NonProfitType = newNonprofitType;
    }
    public override void CashBackRewards()
    {
        double cashBackAmount;
        if (NonProfitType == "E" || NonProfitType == "M") //military or education nonprofits get double the cash-back percentage
        {
            double doubleCashBackPercent = CashBackPercent * 2;
            cashBackAmount = ThisMonthPurchaseAmount * (doubleCashBackPercent / 100);
        }
        else
            cashBackAmount = ThisMonthPurchaseAmount * (CashBackPercent / 100);

        ThisMonthPurchaseAmount = 0;
        Console.WriteLine($"Cash-back reward request for membership {MembershipID} in the amount of ${cashBackAmount:F2} has been made.");
    
    }

    public override string ToString()
    {
        return base.ToString() + $" | Percent cash-back: {CashBackPercent:F2}% | Non-profit type: {NonProfitType}";
    }
}  //end class