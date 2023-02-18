using Microsoft.AspNetCore.Http;
using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Files.Shared.FileRequests;

namespace Messenger.Files.Services;

public interface IFileLocationServer
{
    public Task<IResult> ServeFileAsync<T>(
        SystemFile file,
        T fileRequest,
        HttpContext context,
        CancellationToken cancellationToken = default)
        where T : FileRequest;
}

public interface IFileLocationServer<T> : IFileLocationServer
    where T : IFileLocation
{
}