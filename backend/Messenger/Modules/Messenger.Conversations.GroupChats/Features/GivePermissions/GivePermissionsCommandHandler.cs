using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.GivePermissions;

public class GivePermissionsCommandHandler : ICommandHandler<GivePermissionsCommand, bool>
{
    private readonly IDbContext _dbContext;

    public GivePermissionsCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(GivePermissionsCommand request, CancellationToken cancellationToken)
    {
        (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.FromUserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow()
            .CheckForPermissionsAndThrow(GroupMemberPermissions.AddAdmins);

        var toUser =
            (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.ToUserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow();

        if (toUser.IsOwner)
            throw new ForbiddenException("Can't change owner's permissions");

        var permissions = request.Permissions.Aggregate(
            GroupMemberPermissions.None,
            (currentPermissions, permission) => currentPermissions | permission);

        toUser.Permissions = permissions;
        if (request.MakeAdmin)
            toUser.IsAdmin = true;

        await _dbContext.SaveEntitiesAsync(cancellationToken);
        
        return true;
    }
}
