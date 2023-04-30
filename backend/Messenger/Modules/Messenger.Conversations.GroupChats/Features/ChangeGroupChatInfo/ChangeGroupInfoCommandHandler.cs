using MediatR;
using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.GroupChats.Features.ChangeGroupChatInfo;

public class ChangeGroupInfoCommandHandler : IRequestHandler<ChangeGroupInfoCommand, bool>
{
    private readonly IDbContext _dbContext;

    public ChangeGroupInfoCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(ChangeGroupInfoCommand request, CancellationToken cancellationToken)
    {
        var chatInfo = await _dbContext.GroupChatInfos.FirstOrNotFoundAsync(
            info => info.ConversationId == request.ConversationId,
            cancellationToken: cancellationToken);

        (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.UserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow()
            .CheckForPermissionsAndThrow(GroupMemberPermissions.ChangeGroupInfo);

        chatInfo.Description = request.NewDescription;
        
        //TODO изменение картинки чата
        await _dbContext.SaveEntitiesAsync(cancellationToken);
        
        return true;
    }
}
