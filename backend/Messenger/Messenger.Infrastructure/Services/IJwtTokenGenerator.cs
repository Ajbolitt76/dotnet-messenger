using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Infrastructure.Services;

public interface IJwtTokenGenerator
{
    string GenerateFromClaims(IEnumerable<Claim> claims, DateTime expiresAt);
    
    string GenerateToken<T>(T data, DateTime expiresAt);
    
    T? ReadToken<T>(string token, bool shouldThrow = false, TokenValidationParameters? validationParameters = null);
    
    ClaimsPrincipal ReadToken(string token, TokenValidationParameters? validationParameters = null);
    
    TokenValidationParameters CloneParameters();
}