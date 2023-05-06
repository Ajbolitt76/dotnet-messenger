namespace Messenger.Core.Model.SubscriptionAggregate;

public class UserSubscription : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public uint Plan { get; set; } 
}
