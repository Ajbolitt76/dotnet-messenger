using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.ChangeGroupChatInfo;

public record ChangeGroupInfoCommand(Guid UserId, Guid ConversationId, string NewDescription) : ICommand<bool>;
