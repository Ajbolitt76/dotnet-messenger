using Messenger.S3.Models;

namespace Messenger.S3.Services;

public interface IS3Uploader
{
    Task<UploadResult> PutFileStream(
        Guid fileId,
        long fileSize,
        Stream data,
        CancellationToken cancellationToken);
}
