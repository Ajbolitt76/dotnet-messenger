namespace Messenger.Infrastructure.User;

public record RefreshToken(string Token, DateTime CreatedAt, DateTime Expires, string CreatedByIp, string UserAgent)
{
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => !IsExpired;
}