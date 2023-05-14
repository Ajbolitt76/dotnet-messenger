using Messenger.Core.Requests.Abstractions;

namespace Messenger.User.Feature.SetUserStatus;

public record SetUserStatusCommand(Guid UserId, string Status) : ICommand<bool>;