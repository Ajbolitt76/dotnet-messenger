using Messenger.Core.Model.FileAggregate;
using Messenger.S3.Models;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Messenger.S3.Services;

public class S3Uploader : IS3Uploader
{
    private readonly IMinioClient _minioClient;
    private readonly S3Options _options;

    public S3Uploader(IMinioClient minioClient, IOptions<S3Options> s3Options)
    {
        _minioClient = minioClient;
        _options = s3Options.Value;
    }

    public async Task<UploadResult> PutFileStream(
        Guid fileId,
        long fileSize,
        Stream data,
        CancellationToken cancellationToken)
    {
        var putCommand = new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(fileId.ToString())
            .WithObjectSize(fileSize)
            .WithStreamData(data);

        var putResult = await _minioClient.PutObjectAsync(putCommand, cancellationToken);

        return new UploadResult(_options.BucketName, putResult.ObjectName);
    }
}
