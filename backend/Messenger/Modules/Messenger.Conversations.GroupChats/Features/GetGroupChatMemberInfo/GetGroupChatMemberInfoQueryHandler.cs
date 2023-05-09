using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.GroupChats.Features.GetGroupChatMemberInfo;

public class GetGroupChatMemberInfoQueryHandler : IQueryHandler<GetGroupChatMemberInfoQuery, GetGroupChatMemberInfoResponse>
{
    private readonly IDbContext _dbContext;

    public GetGroupChatMemberInfoQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetGroupChatMemberInfoResponse> Handle(GetGroupChatMemberInfoQuery request, CancellationToken cancellationToken)
    {
        (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.CurrentUserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow();

        var member = await _dbContext.GroupChatMembers.FirstOrNotFoundAsync(
            member => member.ConversationId == request.ConversationId && member.UserId == request.UserId,
            cancellationToken: cancellationToken);

        //TODO маппинг
        return new GetGroupChatMemberInfoResponse(
            member.UserId,
            member.ConversationId,
            member.WasExcluded,
            member.WasBanned,
            member.MutedTill,
            member.Permissions,
            member.IsAdmin,
            member.IsOwner);
    }
}
