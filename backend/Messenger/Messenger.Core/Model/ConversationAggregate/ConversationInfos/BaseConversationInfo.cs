﻿namespace Messenger.Core.Model.ConversationAggregate.ConversationInfos;

public abstract class BaseConversationInfo : BaseEntity, IHaveDiscriminator
{
    public required Guid ConversationId { get; set; }
    
    public Conversation? Conversation { get; set; }
    
    public DateTime LastUpdated { get; set; }
}
