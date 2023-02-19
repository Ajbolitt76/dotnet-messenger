using System.Security.Claims;
using Messenger.Auth.Models;
using Messenger.Core.Model.UserAggregate;
using Messenger.Infrastructure.Services;

namespace Messenger.Auth.Services;

public static class JwtGeneratorExtensions
{
    public static string GenerateUserToken(this IJwtTokenGenerator generator, RepetUser user, DateTime expiraion) 
        => generator.GenerateFromClaims(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        }, expiraion);

    public static TicketMetadata? ReadPhoneTicketRequest(this IJwtTokenGenerator generator, string token)
        => generator.ReadToken<TicketMetadata>(token, false);
}