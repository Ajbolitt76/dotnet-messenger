using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.ExcludeGroupMember;

public class ExcludeGroupMemberCommandHandler : ICommandHandler<ExcludeGroupMemberCommand, bool>
{
    private readonly IDbContext _dbContext;

    public ExcludeGroupMemberCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(ExcludeGroupMemberCommand request, CancellationToken cancellationToken)
    {
        var fromUser =
            (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.FromUserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow()
            .CheckForPermissionsAndThrow(GroupMemberPermissions.BanMembers);

        var toUser = await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(
            request.ToUserId,
            request.ConversationId);

        if ((toUser.IsAdmin && !fromUser.IsOwner) || toUser.IsOwner)
            throw new ForbiddenException(ForbiddenErrorCodes.CantExcludeAdmin);

        if (request.Ban)
            toUser.WasBanned = true;
        
        toUser.WasExcluded = true;

        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}
