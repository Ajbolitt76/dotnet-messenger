using Messenger.SubscriptionPlans.Enums;

namespace Messenger.User.Feature.GetUserSubscriptionPlan;

public class GetUserSubscriptionPlanQueryResponse
{
    public Plan Plan { get; set; }
    public decimal? PricePerMonth { get; set; }
    public uint MessageCharsLimit { get; set; }
    public uint AttachmentsCostPerMessage { get; set; }
    public bool IsStatusEnabled { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}