namespace Messenger.User.Feature.GetUserSubscriptionPlan;

public class GetUserSubscriptionPlanQueryResponse
{
    // 0 если без подписки или срок истек
    public uint Plan { get; set; }
    public decimal? PricePerMonth { get; set; }
    public uint MessageCharsLimit { get; set; }
    public uint UploadPointsAmtMonthLimit { get; set; }
    public bool IsStatusEnabled { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}