using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Extensions;
using tusdotnet;


namespace Messenger.Files;

public class ModuleRoutes : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapTus(
                "/file",
                async ctx =>
                {
                    var uploadManager = ctx.RequestServices.GetRequiredService<TusUploadManager>();
                    return uploadManager.BuildConfiguration();
                })
            .WithDescription("Upload files");

        endpoints.MapGroup("/file")
            .AddEndpoint<ServeFile.ServeFile>();
    }
}
