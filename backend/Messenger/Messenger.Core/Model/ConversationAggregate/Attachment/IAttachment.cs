namespace Messenger.Core.Model.ConversationAggregate.Attachment;

public interface IAttachment : IHaveDiscriminator
{
    /// <summary>
    /// Стоимость вложения в баллах. Нужна для ограничения действий пользователя
    /// </summary>
    public double Cost { get; }
}
