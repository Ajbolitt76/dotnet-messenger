namespace Messenger.Core.Model.ConversationAggregate.ConversationInfos;

public class GroupChatInfo : BaseConversationInfo
{
    /// <summary>
    /// Дискриминатор типа перписки
    /// </summary>
    public static string Discriminator => "Group";
    
    public Guid GroupPictureId { get; set; }
    
    public string? Description { get; set; }
}
