namespace Messenger.Core.Model.ConversationAggregate.Members;

public class ChannelMember : BaseMember
{
    /// <summary>
    /// Является ли админом
    /// </summary>
    public bool IsAdmin { get; set; }
    
    /// <summary>
    /// Битовое поле с разрешениями
    /// </summary>
    public int Permissions { get; set; }
}
