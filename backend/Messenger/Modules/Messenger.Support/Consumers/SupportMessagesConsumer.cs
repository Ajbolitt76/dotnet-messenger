using MassTransit;
using Messenger.Support.MessageActions.SupportSendMessage;
using Messenger.Support.Models;

namespace Messenger.Support.Consumers;

public class SupportMessagesConsumer : IConsumer<SupportMessage>
{
    private readonly ISupportSendMessageActionHandler _handler;

    public SupportMessagesConsumer(ISupportSendMessageActionHandler handler)
    {
        _handler = handler;
    }

    public async Task Consume(ConsumeContext<SupportMessage> context)
    {
        Console.WriteLine(context.Message.Message);
        await _handler.Handle(context.Message, new CancellationToken());
    }
}
