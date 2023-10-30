using MassTransit;
using Messenger.Rabbit.Contracts;
using Messenger.Support.Api.MessageActions.SupportStoreMessage;

namespace Messenger.Support.Api.Consumers;

public class SupportStoreMessagesConsumer : IConsumer<SupportStoreMessageRequest>
{
    private readonly ISupportStoreMessageActionHandler _handler;

    public SupportStoreMessagesConsumer(ISupportStoreMessageActionHandler handler)
    {
        _handler = handler;
    }

    public async Task Consume(ConsumeContext<SupportStoreMessageRequest> context)
    {
        Console.WriteLine(context.Message.Content);
        await _handler.Handle(context.Message, new CancellationToken());
    }
}
