using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.UnbanGroupMember;

public class UnbanGroupMemberCommandHandler : ICommandHandler<UnbanGroupMemberCommand, bool>
{
    private readonly IDbContext _dbContext;

    public UnbanGroupMemberCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UnbanGroupMemberCommand request, CancellationToken cancellationToken)
    {
        (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.FromUserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow()
            .CheckForPermissionsAndThrow(GroupMemberPermissions.BanMembers);
        
        var member = await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(
            request.ToUserId,
            request.ConversationId);

        member.WasBanned = false;
        member.WasExcluded = false;

        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}
