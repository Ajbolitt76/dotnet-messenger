using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Messenger.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Messenger.Core.Services;

namespace Messenger.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Guid? UserId => Guid.TryParse(
        _httpContextAccessor.HttpContext
            ?.User
            ?.FindFirstValue(ClaimTypes.NameIdentifier),
        out var userId)
        ? userId
        : null;

    public bool IsAuthenticated => UserId != null;
    
    public Guid GetUserIdOrThrow() => UserId ?? throw new UnauthorizedException("Unauthorized");

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
