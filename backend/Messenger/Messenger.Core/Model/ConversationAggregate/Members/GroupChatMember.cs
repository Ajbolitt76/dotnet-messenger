using Messenger.Core.Model.UserAggregate;

namespace Messenger.Core.Model.ConversationAggregate.Members;

public class GroupChatMember : BaseMember
{
    /// <summary>
    /// Был ли исключен?
    /// </summary>
    public bool WasExcluded { get; set; }
    
    /// <summary>
    /// Был ли забанен?
    /// </summary>
    public bool WasBanned { get; set; }
    
    /// <summary>
    /// Когда можно писать следующее сообщение
    /// </summary>
    public DateTime MutedTill { get; set; }
    
    /// <summary>
    /// Битовое поле с разрешениями
    /// </summary>
    public int Permissions { get; set; }
}
