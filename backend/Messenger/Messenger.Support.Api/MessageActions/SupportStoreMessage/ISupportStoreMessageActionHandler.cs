using Messenger.Rabbit.Contracts;

namespace Messenger.Support.Api.MessageActions.SupportStoreMessage;

public interface ISupportStoreMessageActionHandler
{
    public Task<bool> Handle(SupportStoreMessageRequest request, CancellationToken cancellationToken);
}
