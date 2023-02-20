namespace Messenger.Core.Model.ConversationAggregate;

public class ConversationMessageMetadata
{
    public static readonly ConversationMessageMetadata Default = new();
    
    public Guid? ForwardedFrom { get; set; }
    
    /// <summary>
    /// Снэпшот имени
    /// </summary>
    public string? ForwardedFromName { get; set; }
    
    public bool IsForwarded => ForwardedFrom != null || ForwardedFromName != null;

    public Guid? ReplyTo { get; set; }
    
    public string? ReplyToName { get; set; }

    public bool IsReply => ReplyTo != null || ReplyToName != null;
}
