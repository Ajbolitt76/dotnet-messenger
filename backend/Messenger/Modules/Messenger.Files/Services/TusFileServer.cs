using Microsoft.AspNetCore.Http;
using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Files.Shared.FileRequests;

namespace Messenger.Files.Services;

public class TusFileServer : IFileLocationServer<TusFileLocation>
{
    private readonly TusUploadManager _tusUploadManager;

    public TusFileServer(TusUploadManager tusUploadManager)
    {
        _tusUploadManager = tusUploadManager;
    }

    public async Task<IResult> ServeFileAsync<T>(
        SystemFile file,
        T fileRequest,
        HttpContext context,
        CancellationToken cancellationToken = default) where T : FileRequest
    {
        if (file.FileLocation is not TusFileLocation tusFileLocation)
            throw new ArgumentException("File request must be of type TusFileRequest");

        var store = _tusUploadManager.GetStore();
        var tusFile = await store.GetFileAsync(tusFileLocation.TusId, cancellationToken);

        if (tusFile is null)
            return Results.NotFound("File not found");
        
        var contentType = "application/octet-stream";
        string? fileDownloadName = file.FileName;
        
        if (fileRequest is ImageFileRequest)
        {
            contentType = "image/jpg";
            fileDownloadName = null;
        }

        return Results.Stream(
            await tusFile.GetContentAsync(cancellationToken),
            contentType,            
            fileDownloadName: fileDownloadName);
    }
}
