using System.Web;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Messenger.Core.Requests.Abstractions;
using Messenger.Crypto.Models;
using Messenger.Crypto.Services;
using Messenger.Files.Services;
using Messenger.Files.Shared;
using Messenger.Files.Shared.Extensions;
using Messenger.Files.Shared.FileRequests;
using Messenger.Files.Shared.FileRequests.Dto;
using Messenger.Infrastructure.Endpoints;
using FileInfo = Messenger.Files.Shared.FileRequests.FileInfo;

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

        endpoints.MapPost(
            "/create-link",
            async (
                [FromBody] SignedData<FileInfo> fileLocation,
                [FromServices] ModuleSignatureService<FileCoreModule> signatureService) =>
            {
                if (!signatureService.Validate(fileLocation))
                {
                    return Results.Forbid();
                }

                var signed = signatureService.Sign(
                    new FileRequest()
                    {
                        FileId = fileLocation.Data.FileId,
                        Expiry = DateTime.UtcNow.AddMinutes(5)
                    });
                return Results.Ok(signed.ToQuery());
            });
    }
}
