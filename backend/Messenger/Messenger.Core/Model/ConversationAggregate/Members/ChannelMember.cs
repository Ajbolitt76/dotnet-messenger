using Messenger.Core.Model.ConversationAggregate.Permissions;

namespace Messenger.Core.Model.ConversationAggregate.Members;

public class ChannelMember : BaseMember
{
    /// <summary>
    /// Является ли админом
    /// </summary>
    public bool IsAdmin { get; set; }
    
    /// <summary>
    /// Является ли владельцем
    /// </summary>
    public bool IsOwner { get; set; }
    
    /// <summary>
    /// Битовое поле с разрешениями
    /// </summary>
    public ChannelMemberPermissions Permissions { get; set; }
}
