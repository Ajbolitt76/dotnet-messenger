using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.GroupChats.Features.GetGroupChatInfo;

public class GetGroupChatInfoQueryHandler : IQueryHandler<GetGroupChatInfoQuery, GetGroupChatInfoQueryResponse>
{
    private readonly IDbContext _dbContext;

    public GetGroupChatInfoQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetGroupChatInfoQueryResponse> Handle(GetGroupChatInfoQuery request, CancellationToken cancellationToken)
    {
        (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.UserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow();

        return await _dbContext.Conversations
            .Where(conversation => conversation.Id == request.ConversationId)
            .Join(
                _dbContext.GroupChatInfos,
                conversation => conversation.Id,
                info => info.ConversationId,
                (conversation, info) => new {Conversation = conversation, Info = info})
            .Select(
                result => new GetGroupChatInfoQueryResponse(
                    result.Conversation.Id,
                    result.Conversation.Title!,
                    result.Info.Description,
                    result.Info.GroupPictureId,
                    result.Info.LastUpdated))
            .FirstOrNotFoundAsync(cancellationToken: cancellationToken);
    }
}
