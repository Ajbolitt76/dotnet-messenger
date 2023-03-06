using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Requests.Responses;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.Channel.Features.CreateChannel;

public class CreateChannelCommandHandler : ICommandHandler<CreateChannelCommand, CreatedResponse<Guid>>
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateChannelCommandHandler(IDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CreatedResponse<Guid>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {	
        var creator = await _dbContext.MessengerUsers.FirstOrNotFoundAsync(
            x => x.Id == request.InitiatorId,
            cancellationToken: cancellationToken);
        
        var conversation = new Conversation
        {
            Title = request.Name, 
            ConversationType = ChannelInfo.Discriminator,
            CreatedAt = _dateTimeProvider.NowUtc, 
            LastMessage = _dateTimeProvider.NowUtc
        };

        _dbContext.Conversations.Add(conversation);

        var channelInfo = new ChannelInfo
        {
            ConversationId = conversation.Id
        };

        _dbContext.ChannelInfos.Add(channelInfo);
        
        var creatorMember = new ChannelMember
        {
            ConversationId = conversation.Id,
            UserId = creator.Id,
            IsAdmin = true,
            Permissions = ChannelPermissionPresets.Creator
        };
        
        _dbContext.ChannelMembers.Add(creatorMember);
        await _dbContext.SaveEntitiesAsync(cancellationToken);
        
        return new CreatedResponse<Guid>(true, conversation.Id);
    }
}
