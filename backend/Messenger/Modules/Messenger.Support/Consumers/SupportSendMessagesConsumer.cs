using MassTransit;
using Messenger.Rabbit.Contracts;
using Messenger.Support.MessageActions.SupportSendMessage;

namespace Messenger.Support.Consumers;


public class SupportSendMessagesConsumer : IConsumer<SupportSendMessageRequest>
{
    private readonly ISupportSendMessageActionHandler _handler;

    public SupportSendMessagesConsumer(ISupportSendMessageActionHandler handler)
    {
        _handler = handler;
    }

    public async Task Consume(ConsumeContext<SupportSendMessageRequest> context)
    {
        Console.WriteLine(context.Message.Message);
        await _handler.Handle(context.Message, new CancellationToken());
    }
}
