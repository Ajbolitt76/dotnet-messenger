using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Messenger.Core.Requests.Abstractions;
using Messenger.Crypto.Models;
using Messenger.Files.Services;
using Messenger.Files.Shared.FileRequests;
using Messenger.Files.Shared.FileRequests.Dto;
using Messenger.Infrastructure.Endpoints;

namespace Messenger.Files.ServeFile;

public class ServeFile : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/",
            async (
                    [AsParameters] FileRequestQuery fileRequest,
                    [FromQuery(Name = nameof(SignedData<FileRequest>.Signature))]
                    string signature,
                    [FromServices] FileServer fileService,
                    HttpContext context,
                    CancellationToken cancellationToken)
                =>
            {
                var signedData = new SignedData<FileRequest>(fileRequest.ToEntity(), signature);

                return await fileService.ServeFileAsync(signedData, context, cancellationToken);
            });

        endpoints.MapGet(
            "/picture",
            async (
                [AsParameters] ImageFileRequestQuery fileRequest,
                [FromQuery(Name = nameof(SignedData<FileRequest>.Signature))]
                string signature,
                [FromServices] FileServer fileService,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var signedData = new SignedData<ImageFileRequest>(fileRequest.ToEntity(), signature);

                return await fileService.ServeFileAsync(signedData, context, cancellationToken);
            });
    }
}
