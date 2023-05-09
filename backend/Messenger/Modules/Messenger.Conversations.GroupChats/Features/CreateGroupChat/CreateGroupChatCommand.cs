using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Requests.Responses;

namespace Messenger.Conversations.GroupChats.Features.CreateGroupChat;

public record CreateGroupChatCommand(
    Guid InitiatorId,
    IEnumerable<Guid> InvitedMemberIds,
    string Name) : ICommand<GroupCreatedResponse>;