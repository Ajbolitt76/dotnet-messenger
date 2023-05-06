using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Features.SendMessageCommand;

public record SendMessageCommand(Guid ConversationId, Guid UserId, string TextContent) : ICommand<SendMessageCommandResponse>;