namespace Messenger.Core.Model.ConversationAggregate;

public class ConversationUserStatus : BaseEntity
{
    public Guid? ConversationId { get; set; }

    public Guid? UserId { get; set; }
    
    public uint? ReadTo { get; set; }
    
    public uint? DeletedTo { get; set; }
    
    public bool IsDeletedByUser { get; set; }
}
