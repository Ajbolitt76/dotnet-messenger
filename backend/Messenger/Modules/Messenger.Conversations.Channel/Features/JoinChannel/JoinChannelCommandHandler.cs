using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.Channel.Features.JoinChannel;

public class JoinChannelCommandHandler : ICommandHandler<JoinChannelCommand, bool>
{
    private readonly IDbContext _dbContext;

    public JoinChannelCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(JoinChannelCommand request, CancellationToken cancellationToken)
    {
        // Проверка, что переписка является каналом
        await _dbContext.Conversations.FirstOrNotFoundAsync(
            conversation => conversation.Id == request.ConversationId
                            && conversation.ConversationType == ConversationTypes.Channel,
            cancellationToken: cancellationToken);

        var status = await _dbContext.ConversationUserStatuses.FirstOrDefaultAsync(
            status => status.ConversationId == request.ConversationId && status.UserId == request.CurrentUserId,
            cancellationToken: cancellationToken);

        if (status is not null)
        {
            status.IsDeletedByUser = false;
            await _dbContext.SaveEntitiesAsync(cancellationToken);
            return true;
        }
        
        var newMember = new ChannelMember
        {
            ConversationId = request.ConversationId,
            UserId = request.CurrentUserId,
            IsAdmin = false,
            IsOwner = false,
            Permissions = ChannelPermissionPresets.Member
        };
        
        var newStatus = new ConversationUserStatus
        {
            ConversationId = request.ConversationId,
            UserId = request.CurrentUserId,
            ReadTo = -1,
            DeletedTo = null,
            SoftDeletedCount = 0,
            IsDeletedByUser = false
        };
        
        _dbContext.ChannelMembers.Add(newMember);
        _dbContext.ConversationUserStatuses.Add(newStatus);
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}
