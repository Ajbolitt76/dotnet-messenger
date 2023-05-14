using Messenger.Conversations.Channel.Features.ChangeChannelInfo;
using Messenger.Conversations.Channel.Features.CreateChannel;
using Messenger.Conversations.Channel.Features.GetChannelInfo;
using Messenger.Conversations.Channel.Features.GiveChannelPermissions;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Channel;

public class ChannelRoutes : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/channel")
            .WithTags("Каналы")
            .AddEndpoint<CreateChannelEndpoint>()
            .AddEndpoint<ChangeChannelInfoEndpoint>()
            .AddEndpoint<GiveChannelPermissionsEndpoint>()
            .AddEndpoint<GetChannelInfoEndpoint>();
    }
}
