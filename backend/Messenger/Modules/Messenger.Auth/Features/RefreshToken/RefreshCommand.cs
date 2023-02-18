using Messenger.Core.Requests.Abstractions;

namespace Messenger.Auth.Features.RefreshToken;

public record RefreshTokenCommand(string Token, string RefreshToken) : ICommand<RefreshTokenResponse>;