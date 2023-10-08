using Messenger.Core.Requests.Abstractions;

namespace Messenger.Support.Features.SendSupportMessage;

public record SendSupportMessageCommand(Guid ConversationId, string TextContent) : ICommand<bool>;