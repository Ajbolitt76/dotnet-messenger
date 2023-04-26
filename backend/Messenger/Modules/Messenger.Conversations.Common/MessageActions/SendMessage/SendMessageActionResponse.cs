namespace Messenger.Conversations.Common.MessageActions.SendMessage;

public record SendMessageActionResponse(bool Sent, Guid MessageId);
