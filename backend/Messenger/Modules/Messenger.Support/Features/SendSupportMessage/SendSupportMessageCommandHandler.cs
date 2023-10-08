using MassTransit;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Messenger.Support.Models;

namespace Messenger.Support.Features.SendSupportMessage;

public partial class SendMessageCommandHandler : ICommandHandler<SendSupportMessageCommand, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IBus _bus;
    private readonly Guid _supportGuid = SupportIdSetup.SupportId;
    private readonly IUserService _userService;

    public SendMessageCommandHandler(IDbContext dbContext, IBus bus, IUserService userService)
    {
        _dbContext = dbContext;
        _bus = bus;
        _userService = userService;
    }

    public async Task<bool> Handle(SendSupportMessageCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _dbContext.Conversations.FirstOrNotFoundAsync(
            x => x.Id == request.ConversationId && x.ConversationType == ConversationTypes.PersonalChat,
            cancellationToken: cancellationToken);
        await _dbContext.PersonalChatInfos.FirstOrNotFoundAsync(
            x => x.ConversationId == conversation.Id
                 && (x.RecipientPeer == _supportGuid || x.InitiatorPeer == _supportGuid),
            cancellationToken: cancellationToken);

        await _bus.Publish(
            new SupportMessage(_userService.GetUserIdOrThrow(), conversation.Id, request.TextContent),
            cancellationToken);
        
        return true;
    }
}
