using Messenger.Conversations.Common.Abstractions;

namespace Messenger.Conversations.Common.MessageActions.GetMessageList;

/// <summary>
/// Получить список сообщекний в беседе
/// </summary>
/// <param name="ConversationId">Id переписки</param>
/// <param name="Count">Количество (положительно берем старше, отрицательно берем новее)</param>
/// <param name="MessagePointer">Указатель на сообщение</param>
public record GetMessageListAction(
    Guid ConversationId,
    int Count = 40,
    Guid? MessagePointer = default) : IMessageAction<GetMessageListActionResponse>;
