using Messenger.Conversations.Common.Models;

namespace Messenger.Conversations.Common.MessageActions.GetMessageList;

public record GetMessageListActionResponse(
    List<BaseMessageListProjection> Messages);
