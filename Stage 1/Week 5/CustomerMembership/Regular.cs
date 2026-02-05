using System;

namespace CustomerMembership;

class Regular : Membership, ISpecialOffer
{
    public double CashBackPercent
        { get; set; }
    
    public Regular() : base()
    {
        AnnualCost = 65;
        MembershipType = "R";
        CashBackPercent = 2;
    }

   // public Regular(double newCost, int newID, string newType, string newEmail, double newPurchaseAmount, double newCashBackPercent) : base(newCost, newID, newType, newEmail, newPurchaseAmount)
    public Regular(int newID, string newEmail, double newPurchaseAmount) : base(newID, newEmail, newPurchaseAmount)
    {
        AnnualCost = 65;
        MembershipType = "R";
        CashBackPercent = 2;
    }

    public double CalculateSpecialOffer()
    {
        double specialOfferReturn= AnnualCost * 0.25;
        return specialOfferReturn;
    }
    public override void CashBackRewards()
    {
        double cashBackAmount = ThisMonthPurchaseAmount * (CashBackPercent / 100);
        ThisMonthPurchaseAmount = 0;
        Console.WriteLine($"Cash-back reward request for membership {MembershipID} in the amount of ${cashBackAmount:F2} has been made.");
    }

    public override string ToString()
    {
        return base.ToString() + $" | Cash Back: {CashBackPercent:F2}% | Special Offer Return: ${CalculateSpecialOffer():F2}";
    }
} //end class