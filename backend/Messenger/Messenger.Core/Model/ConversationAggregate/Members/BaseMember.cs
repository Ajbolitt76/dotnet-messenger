using Messenger.Core.Model.UserAggregate;

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

    #region Navigation

    public Conversation? Conversation { get; set; }
    
    public MessengerUser? MessengerUser { get; set;  }

    #endregion
}
