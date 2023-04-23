using Messenger.Conversations.Common.MessageActions.SendMessage;

namespace Messenger.Conversations.Features.SendMessageCommand;

public record SendMessageCommandResponse(bool Sent, Guid MessageId) : SendMessageActionResponse(Sent, MessageId)
{
    public SendMessageCommandResponse(SendMessageActionResponse actionResponse) : this(
        actionResponse.Sent,
        actionResponse.MessageId)
    {
    }
};
