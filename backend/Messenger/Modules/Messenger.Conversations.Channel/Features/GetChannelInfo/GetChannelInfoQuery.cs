using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Channel.Features.GetChannelInfo;

public record GetChannelInfoQuery(Guid ConversationId) : IQuery<GetChannelInfoQueryResponse>;