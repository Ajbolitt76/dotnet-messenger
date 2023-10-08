using Messenger.Support.Models;

namespace Messenger.Support.MessageActions.SupportSendMessage;

public interface ISupportSendMessageActionHandler
{
    public Task<bool> Handle(SupportMessage request, CancellationToken cancellationToken);
}
