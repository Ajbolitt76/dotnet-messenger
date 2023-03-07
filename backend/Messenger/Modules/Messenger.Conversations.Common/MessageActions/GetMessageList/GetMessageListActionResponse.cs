using Messenger.Conversations.Common.Models;

namespace Messenger.Conversations.Common.MessageActions.GetMessageList;

public record GetMessageListActionResponse(
    IReadOnlyCollection<BaseMessageListProjection> Messages);
