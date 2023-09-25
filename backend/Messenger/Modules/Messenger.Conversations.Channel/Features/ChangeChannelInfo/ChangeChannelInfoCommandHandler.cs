using Messenger.Conversations.Channel.Extensions;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.Channel.Features.ChangeChannelInfo;

public class ChangeChannelInfoCommandHandler : ICommandHandler<ChangeChannelInfoCommand, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ChangeChannelInfoCommandHandler(IDateTimeProvider dateTimeProvider, IDbContext dbContext)
    {
        _dateTimeProvider = dateTimeProvider;
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(ChangeChannelInfoCommand request, CancellationToken cancellationToken)
    {
        (await _dbContext.ChannelMembers.GetChannelMemberOrThrowAsync(request.UserId, request.ConversationId))
            .CheckForPermissionsAndThrow(ChannelMemberPermissions.ChangeChannelInfo);
        
        var chatInfo = await _dbContext.ChannelInfos.FirstOrNotFoundAsync(
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
