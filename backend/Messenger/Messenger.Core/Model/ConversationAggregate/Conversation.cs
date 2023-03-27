using Messenger.Core.Model.ConversationAggregate.ConversationInfos;

namespace Messenger.Core.Model.ConversationAggregate;

public class Conversation : BaseEntity
{
    /// <summary>
    /// Заголовок
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Время последнего сообщения
    /// </summary>
    public required DateTime LastMessage { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дискриминатор типа переписки
    /// </summary>
    public required string ConversationType { get; init; }
    
    /// <summary>
    /// Общее количество удаленных сообщений
    /// </summary>
    public uint HardDeletedCount { get; set; }

    /// <summary>
    /// Переписка
    /// </summary>
    public BaseConversationInfo? ConversationInfo { get; private set; }
    
    public void SetConversationInfo<T>(T conversationInfo) where T: BaseConversationInfo, IHaveDiscriminator
    {
        if (ConversationType != T.Discriminator)
            throw new InvalidOperationException("Нельзя изменить тип переписки");
        ConversationInfo = conversationInfo;
    }
}
