namespace Messenger.SubscriptionPlans.Models;

public class PlanDetails
{
    public decimal PricePerMonth { get; set; }
    public uint MessageCharsLimit { get; set; }
    public uint AttachmentsCostPerMessage { get; set; }
    public bool IsStatusEnabled { get; set; }

    public PlanDetails(decimal pricePerMonth, uint messageCharsLimit, uint attachmentsCostPerMessage, bool isStatusEnabled)
    {
        PricePerMonth = pricePerMonth;
        MessageCharsLimit = messageCharsLimit;
        AttachmentsCostPerMessage = attachmentsCostPerMessage;
        IsStatusEnabled = isStatusEnabled;
    }
}
