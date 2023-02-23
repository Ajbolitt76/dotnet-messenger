using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;

public record ReserveConversationNumberCommand(Guid ConversationId) : ICommand<uint>;