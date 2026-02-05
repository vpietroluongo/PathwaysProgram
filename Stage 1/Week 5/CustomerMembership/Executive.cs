using System;

namespace CustomerMembership;

class Executive : Membership, ISpecialOffer
{
    public double CashBackPercentAbove1000   
        { get; set; }
    public double CashBackPercentBelow1000   
        { get; set; }
    
    public Executive() : base()
    {
        AnnualCost = 130;
        CashBackPercentAbove1000 = 3;
        CashBackPercentBelow1000 = 1.5;
        MembershipType = "E";
    }

    public Executive(int newID, string newEmail, double newPurchaseAmount) : base(newID, newEmail, newPurchaseAmount)
    {
        AnnualCost = 130;
        CashBackPercentAbove1000 = 3;
        CashBackPercentBelow1000 = 1.5;
        MembershipType = "E";
    }

    public double CalculateSpecialOffer()
    {
        double specialOfferReturn = AnnualCost * 0.5;
        return specialOfferReturn;
    }
    public override void CashBackRewards()
    {
        double cashBackAmount;
        if (ThisMonthPurchaseAmount > 1000)
            cashBackAmount = ThisMonthPurchaseAmount * (CashBackPercentAbove1000 / 100);
        else
            cashBackAmount = ThisMonthPurchaseAmount * (CashBackPercentBelow1000 / 100);

        ThisMonthPurchaseAmount = 0;
        Console.WriteLine($"Cash-back reward request for membership {MembershipID} in the amount of ${cashBackAmount:F2} has been made.");
    }
    public override string ToString()
    {
        return base.ToString() + $" | Percent cash-back >= 1000: {CashBackPercentAbove1000:F2}% | Percent cash-back <1000 = {CashBackPercentBelow1000:F2}% | Special Offer Return: ${CalculateSpecialOffer():F2}";
    }
} //end class