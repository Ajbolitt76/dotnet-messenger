using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Channel.Features.GetChannelMemberInfo;

public record GetChannelMemberInfoQuery(
    Guid UserId,
    Guid ConversationId) : IQuery<GetChannelMemberInfoResponse>;
