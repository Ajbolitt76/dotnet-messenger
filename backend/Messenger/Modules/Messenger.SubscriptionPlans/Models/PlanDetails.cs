namespace Messenger.SubscriptionPlans.Models;

public class PlanDetails
{
    public decimal PricePerMonth { get; set; }
    public uint MessageCharsLimit { get; set; }
    public uint UploadPointsAmtMonthLimit { get; set; }
    public bool IsStatusEnabled { get; set; }

    public PlanDetails(decimal pricePerMonth, uint messageCharsLimit, uint mediaAmtMonthLimit, bool isStatusEnabled)
    {
        PricePerMonth = pricePerMonth;
        MessageCharsLimit = messageCharsLimit;
        UploadPointsAmtMonthLimit = mediaAmtMonthLimit;
        IsStatusEnabled = isStatusEnabled;
    }
}
