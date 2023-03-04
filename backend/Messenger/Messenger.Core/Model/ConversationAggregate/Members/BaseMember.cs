namespace Messenger.Core.Model.ConversationAggregate.Members;

public abstract class BaseMember : BaseEntity
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Id переписки
    /// </summary>
    public Guid ConversationId { get; set; }
}
