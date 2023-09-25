using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.Channel.Features.GetChannelInfo;

public class GetChannelInfoQueryHandler : IQueryHandler<GetChannelInfoQuery, GetChannelInfoQueryResponse>
{
    private readonly IDbContext _dbContext;

    public GetChannelInfoQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetChannelInfoQueryResponse> Handle(GetChannelInfoQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Conversations
            .Where(conversation => conversation.Id == request.ConversationId)
            .Join(
                _dbContext.ChannelInfos,
                conversation => conversation.Id,
                info => info.ConversationId,
                (conversation, info) => new {Conversation = conversation, Info = info})
            .Select(
                result => new GetChannelInfoQueryResponse(
                    result.Conversation.Id,
                    result.Conversation.Title!,
                    result.Info.Description,
                    result.Info.ChannelPictureId,
                    result.Info.LastUpdated))
            .FirstOrNotFoundAsync(cancellationToken: cancellationToken);
    }
}
