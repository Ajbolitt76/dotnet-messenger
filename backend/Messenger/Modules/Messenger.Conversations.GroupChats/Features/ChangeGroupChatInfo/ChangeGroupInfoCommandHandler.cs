using MediatR;
using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.GroupChats.Features.ChangeGroupChatInfo;

public class ChangeGroupInfoCommandHandler : IRequestHandler<ChangeGroupInfoCommand, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ChangeGroupInfoCommandHandler(IDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<bool> Handle(ChangeGroupInfoCommand request, CancellationToken cancellationToken)
    {
        (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.UserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow()
            .CheckForPermissionsAndThrow(GroupMemberPermissions.ChangeGroupInfo);
        
        var chatInfo = await _dbContext.GroupChatInfos.FirstOrNotFoundAsync(
            info => info.ConversationId == request.ConversationId,
            cancellationToken: cancellationToken);

        var conversation = await _dbContext.Conversations.FirstOrNotFoundAsync(
            conversation => conversation.Id == request.ConversationId,
            cancellationToken: cancellationToken);

        conversation.Title = request.NewTitle;
        chatInfo.Description = request.NewDescription;
        chatInfo.LastUpdated = _dateTimeProvider.NowUtc;
        
        //TODO изменение картинки чата
        await _dbContext.SaveEntitiesAsync(cancellationToken);
        
        return true;
    }
}
