using Messenger.Core;
using Messenger.Core.Exceptions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.Channel.Features.LeaveChannel;

public class LeaveChannelCommandHandler : ICommandHandler<LeaveChannelCommand, bool>
{
    private readonly IDbContext _dbContext;

    public LeaveChannelCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(LeaveChannelCommand request, CancellationToken cancellationToken)
    {
        var member = await _dbContext.ChannelMembers.FirstOrNotFoundAsync(
            member => member.ConversationId == request.ConversationId && member.UserId == request.CurrentUserId,
            cancellationToken: cancellationToken);

        if (member.IsOwner)
            throw new ForbiddenException(ForbiddenErrorCodes.OwnerCantLeaveChannel);
        
        var status = await _dbContext.ConversationUserStatuses.FirstOrNotFoundAsync(
            status => status.ConversationId == request.ConversationId && status.UserId == request.CurrentUserId,
            cancellationToken: cancellationToken);

        _dbContext.ChannelMembers.Remove(member);
        _dbContext.ConversationUserStatuses.Remove(status);
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}
