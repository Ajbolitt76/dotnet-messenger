using MediatR;
using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;

namespace Messenger.Conversations.GroupChats.Features.MuteGroupMember;

public class MuteGroupMemberCommandHandler : IRequestHandler<MuteGroupMemberCommand, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MuteGroupMemberCommandHandler(IDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<bool> Handle(MuteGroupMemberCommand request, CancellationToken cancellationToken)
    {
        (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.FromUserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow()
            .CheckForPermissionsAndThrow(GroupMemberPermissions.MuteMembers);

        var toUser =
            (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.ToUserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow();

        toUser.MutedTill = _dateTimeProvider.NowUtc.Add(request.TimeSpan);

        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}
