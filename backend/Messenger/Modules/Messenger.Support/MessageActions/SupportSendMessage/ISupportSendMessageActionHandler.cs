using Messenger.Rabbit.Contracts;

namespace Messenger.Support.MessageActions.SupportSendMessage;

public interface ISupportSendMessageActionHandler
{
    public Task<bool> Handle(SupportSendMessageRequest request, CancellationToken cancellationToken);
}
