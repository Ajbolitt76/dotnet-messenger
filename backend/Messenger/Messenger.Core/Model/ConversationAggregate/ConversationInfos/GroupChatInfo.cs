﻿namespace Messenger.Core.Model.ConversationAggregate.ConversationInfos;

public class GroupChatInfo : BaseConversationInfo, IHaveDiscriminator
{
    /// <summary>
    /// Дискриминатор типа перписки
    /// </summary>
    public static string Discriminator => ConversationTypes.GroupChat;
    
    public Guid GroupPictureId { get; set; }
    
    public string? Description { get; set; }
}
