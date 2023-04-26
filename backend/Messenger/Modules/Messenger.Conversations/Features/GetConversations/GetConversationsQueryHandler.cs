using System.Linq.Expressions;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.Features.GetConversations;

public class GetConversationsQueryHandler : IQueryHandler<GetConversationsQuery, GetConversationsQueryResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;

    public GetConversationsQueryHandler(IDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<GetConversationsQueryResponse> Handle(
        GetConversationsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _userService.GetUserIdOrThrow();
        // EF изнасилован, в ахуе
        var items =
            await GetConversationGuids(_dbContext.GroupChatMembers)
                .Concat(GetConversationGuids(_dbContext.PersonalChatMembers))
                .Concat(GetConversationGuids(_dbContext.ChannelMembers))
                .Select(x => _dbContext.Conversations.First(y => y.Id == x))
                .Select(
                    x => new GetConversationsQueryResponseItem(
                        x.Id,
                        (x.ConversationType == ConversationTypes.PersonalChat
                            ? _dbContext.PersonalChatMembers.FirstOrDefault(
                                    cm =>
                                        cm.ConversationId == x.Id
                                        && cm.UserId != userId)!
                                .MessengerUser!
                                .Name
                            : x.Title) ?? "",
                        _dbContext.ConversationMessages
                            .Where(cm => 
                                cm.ConversationId == x.Id
                                && (cm.DeletedFrom == null || !cm.DeletedFrom.Contains(userId)))
                            .OrderByDescending(cm => cm.Position)
                            .Select(
                                cm => new ConversationLastMessage(
                                    x.ConversationType == ConversationTypes.Channel
                                        ? null
                                        : cm.SenderMessengerUser!.Name,
                                    cm.TextContent,
                                    cm.SentAt))
                            .FirstOrDefault()!,
                        x.ConversationType))
                .ToListAsync(cancellationToken: cancellationToken);

        return new(items);
    }

    private IQueryable<Guid> GetConversationGuids<T>(IQueryable<T> queryable)
        where T : BaseMember
        => queryable
            .Where(x => x.UserId == _userService.GetUserIdOrThrow())
            .Select(x => x.ConversationId);
}
