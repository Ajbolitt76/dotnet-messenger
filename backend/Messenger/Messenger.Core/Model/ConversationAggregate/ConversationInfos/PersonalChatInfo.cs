namespace Messenger.Core.Model.ConversationAggregate.ConversationInfos;

public class PersonalChatInfo : BaseConversationInfo, IHaveDiscriminator
{
    /// <summary>
    /// Дискриминатор типа перписки
    /// </summary>
    public static string Discriminator => "Personal";
    
    public required Guid InitiatorPeer { get; set; }
    
    public required Guid RecipientPeer { get; set; }

    private bool RecipientConfirmed { get; set; }
}
