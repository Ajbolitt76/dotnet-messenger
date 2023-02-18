using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Messenger.Auth.Features.Login;
using Messenger.Auth.Features.PhoneTicket.AcquirePhoneTicket;
using Messenger.Auth.Features.PhoneTicket.GetLoginOrRegisterTicket;
using Messenger.Auth.Features.RefreshToken;
using Messenger.Auth.Features.Registration;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Auth;

public class ModuleRoutes : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/auth")
            .WithTags("Авторизация");
        
        group
            .AddEndpoint<LoginEndpoint>()
            .AddEndpoint<GetLoginOrRegisterTicketEndpoint>()
            .AddEndpoint<RegistrationEndpoint>()
            .AddEndpoint<AcquirePhoneTicketEndpoint>()
            .AddEndpoint<RefreshTokenEndpoint>();
    }
}
