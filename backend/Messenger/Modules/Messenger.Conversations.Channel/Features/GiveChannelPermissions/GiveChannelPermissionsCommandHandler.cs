using Messenger.Conversations.Channel.Extensions;
using Messenger.Core;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Channel.Features.GiveChannelPermissions;

public class GiveChannelPermissionsCommandHandler : ICommandHandler<GiveChannelPermissionsCommand, bool>
{
    private readonly IDbContext _dbContext;

    public GiveChannelPermissionsCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(GiveChannelPermissionsCommand request, CancellationToken cancellationToken)
    {
        (await _dbContext.ChannelMembers.GetChannelMemberOrThrowAsync(request.FromUserId, request.ConversationId))
            .CheckForPermissionsAndThrow(ChannelMemberPermissions.AddAdmins);

        var toUser =
            await _dbContext.ChannelMembers.GetChannelMemberOrThrowAsync(request.ToUserId, request.ConversationId);

        if (toUser.IsOwner)
            throw new ForbiddenException(ForbiddenErrorCodes.CantChangeOwnerPermissions);

        var permissions = request.Permissions.Aggregate(
            ChannelMemberPermissions.None,
            (currentPermissions, permission) => currentPermissions | permission);

        toUser.Permissions = permissions;
        if (request.MakeAdmin)
            toUser.IsAdmin = true;

        await _dbContext.SaveEntitiesAsync(cancellationToken);
        
        return true;
    }
}
