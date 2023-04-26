namespace Messenger.Core.Model.ConversationAggregate;

public class ConversationUserStatus : BaseEntity
{
    public Guid ConversationId { get; set; }

    public Guid UserId { get; set; }

    /// <summary>
    /// Номер последнего прочитанного сообщения
    /// </summary>
    public int ReadTo { get; set; }

    /// <summary>
    /// Номер сообщения, до которого пользователь очистил переписку
    /// </summary>
    public uint? DeletedTo { get; set; }
    
    /// <summary>
    /// Количество мягко удаленых сообщений
    /// </summary>
    public uint SoftDeletedCount { get; set; }
    
    public bool IsDeletedByUser { get; set; }
}
