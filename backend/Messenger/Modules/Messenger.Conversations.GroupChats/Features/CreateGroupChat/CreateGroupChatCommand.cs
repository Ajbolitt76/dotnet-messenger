using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Requests.Responses;

namespace Messenger.Conversations.GroupChats.Features.CreateGroupChat;

public record CreateGroupChatCommand(
    Guid InitiatorId,
    IEnumerable<Guid> InvitedMembers,
    string Name) : ICommand<CreatedResponse<Guid>>;