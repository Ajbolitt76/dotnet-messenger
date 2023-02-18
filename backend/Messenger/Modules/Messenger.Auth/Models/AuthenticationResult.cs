using Messenger.Infrastructure.User;

namespace Messenger.Auth.Models;

public record AuthenticationResult(string Token, RefreshToken RefreshToken);