using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.Channel.Features.GetChannelMemberInfo;

public class GetChannelMemberInfoQueryHandler : IQueryHandler<GetChannelMemberInfoQuery, GetChannelMemberInfoResponse>
{
    private readonly IDbContext _dbContext;

    public GetChannelMemberInfoQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetChannelMemberInfoResponse> Handle(GetChannelMemberInfoQuery request, CancellationToken cancellationToken)
    {
        var member = await _dbContext.ChannelMembers.FirstOrNotFoundAsync(
            member => member.ConversationId == request.ConversationId && member.UserId == request.UserId,
            cancellationToken: cancellationToken);

        return new GetChannelMemberInfoResponse(
            member.UserId,
            member.ConversationId,
            member.Permissions,
            member.IsAdmin,
            member.IsOwner);
    }
}
