using MassTransit;
using Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Messenger.Rabbit.Contracts;

namespace Messenger.Support.Features.SendSupportMessage;

public partial class SendMessageCommandHandler : ICommandHandler<SendSupportMessageCommand, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IDomainHandler<ReserveConversationNumberCommand, uint> _reserveNumberHandler;
    private readonly IPublishEndpoint _bus;
    private readonly Guid _supportGuid = SupportSetup.SupportId;
    private readonly IUserService _userService;

    public SendMessageCommandHandler(IDbContext dbContext, IPublishEndpoint bus, IUserService userService, 
        IDomainHandler<ReserveConversationNumberCommand, uint> reserveNumberHandler)
    {
        _dbContext = dbContext;
        _bus = bus;
        _userService = userService;
        _reserveNumberHandler = reserveNumberHandler;
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
        
        var messagePosition = 
            await _reserveNumberHandler.Handle(new(request.ConversationId), cancellationToken);

        await _bus.Publish(
            new SupportStoreMessageRequest(_userService.GetUserIdOrThrow(), conversation.Id, request.TextContent, messagePosition),
            cancellationToken);
        
        return true;
    }
}
