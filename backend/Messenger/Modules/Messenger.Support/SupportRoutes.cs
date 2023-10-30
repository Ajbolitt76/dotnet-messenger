using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Extensions;
using Messenger.Support.Features.CreateSupportConversation;
using Messenger.Support.Features.SendSupportMessage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Support;

public class SupportRoutes : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/support")
            .WithTags("Поддержка")
            .AddEndpoint<CreateSupportConversationEndpoint>()
            .AddEndpoint<SendSupportMessageEndpoint>();
    }
}
