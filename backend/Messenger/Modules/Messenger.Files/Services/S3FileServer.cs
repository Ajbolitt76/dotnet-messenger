using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Files.Shared.FileRequests;
using Messenger.S3;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Messenger.Files.Services;

public class S3FileServer : IFileLocationServer<S3FileLocation>
{
    private readonly IMinioClient _minioClient;

    public S3FileServer(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }
    
    public async Task<IResult> ServeFileAsync<T>(
        SystemFile file,
        T fileRequest,
        HttpContext context,
        CancellationToken cancellationToken = default) where T : FileRequest
    {
        if (file.FileLocation is not S3FileLocation s3FileLocation)
            throw new ArgumentException("File request must be of type S3FileLocation");

        var type = "attachment";       
        
        if (fileRequest is ImageFileRequest)
        {
            type = "inline";
        }
        
        var req = new PresignedGetObjectArgs()
            .WithBucket(s3FileLocation.BucketName)
            .WithObject(s3FileLocation.Key)
            .WithHeaders(new Dictionary<string, string>()
            {
                ["response-content-disposition"] = $"{type}; filename=\"{file.FileName}\""
            });

        if (fileRequest.Expiry.HasValue)
        {
            req.WithExpiry((fileRequest.Expiry.Value.Subtract(DateTime.UtcNow)).Seconds);
        }

        var url = await _minioClient.PresignedGetObjectAsync(req);
        return Results.Redirect(url);
    }
}
