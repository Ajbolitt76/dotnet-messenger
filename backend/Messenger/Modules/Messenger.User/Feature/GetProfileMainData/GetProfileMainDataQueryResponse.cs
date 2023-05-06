namespace Messenger.User.Feature.GetProfileMainData;

public class GetProfileMainDataQueryResponse
{
    public Guid Id { get; set; }
    public string ProfilePhoto { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string? Status { get; set; }
    public DateTime? DateOfBirth { get; set; }
    // 0 если чел без подписки или срок истек
    public uint SubscriptionPlan { get; set; }
}