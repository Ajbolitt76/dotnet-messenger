using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.BanOrKickGroupMember;

public class BanOrKickGroupMemberCommandHandler : ICommandHandler<BanOrKickGroupMemberCommand, bool>
{
    private readonly IDbContext _dbContext;

    public BanOrKickGroupMemberCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(BanOrKickGroupMemberCommand request, CancellationToken cancellationToken)
    {
        var fromUser =
            (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.FromUserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow()
            .CheckForPermissionsAndThrow(GroupMemberPermissions.BanMembers);

        var toUser = await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(
            request.ToUserId,
            request.ConversationId);

        if ((toUser.IsAdmin && !fromUser.IsOwner) || toUser.IsOwner)
            throw new ForbiddenException("Not enough permissions to kick/ban admin");

        if (request.Penalty == PenaltyScopes.Ban)
            toUser.WasBanned = true;
        
        toUser.WasExcluded = true;

        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}
