public class OrderValidator
{
    public bool IsValidOrder(decimal totalAmount, int itemCount, bool isMember, bool hasCoupon)
    {
        // Rule 1: Order must have positive amount
        if (totalAmount <= 0)
            return false;

        // Rule 2: Must have at least one item
        if (itemCount < 1)
            return false;

        // Rule 3: Minimum order value for non-members
        if (!isMember && totalAmount < 25)
            return false;

        // Rule 4: Special member + coupon logic
        if (isMember && hasCoupon)
        {
            if (totalAmount < 15)
                return false;
            return true; // Members with coupon get lower minimum
        }

        // Rule 5: Regular member minimum
        if (isMember && totalAmount < 20)
            return false;

        return true;
    }
}
