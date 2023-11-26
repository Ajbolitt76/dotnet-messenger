using MediatR;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Core.Requests.Abstractions;
using Messenger.S3.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Files.Features;

public readonly record struct UploadOk();
public readonly record struct UploadNotFound();

public record PushToS3Command(Guid FileId) : ICommand<OneOf<UploadOk, UploadNotFound>>;

public class PushToS3CommandHandler : ICommandHandler<PushToS3Command, OneOf<UploadOk, UploadNotFound>>
{
    private readonly IDbContext _dbContext;
    private readonly IS3Uploader _s3Uploader;
    private readonly TusUploadManager _uploadManager;
    private readonly ILogger<PushToS3CommandHandler> _logger;

    public PushToS3CommandHandler(
        IDbContext dbContext,
        IS3Uploader s3Uploader,
        TusUploadManager uploadManager,
        ILogger<PushToS3CommandHandler> logger)
    {
        _dbContext = dbContext;
        _s3Uploader = s3Uploader;
        _uploadManager = uploadManager;
        _logger = logger;
    }
    
    public async Task<OneOf<UploadOk, UploadNotFound>> Handle(PushToS3Command request, CancellationToken cancellationToken)
    {
        var file = await _dbContext.Files.FirstOrDefaultAsync(
            x => x.Id == request.FileId,
            cancellationToken: cancellationToken);

        if (file is not { FileLocation: TusFileLocation tusFile } )
        {
            return default;
        }
     
        var tusProvidedFile = await _uploadManager.GetStore().GetFileAsync(tusFile.TusId, cancellationToken);
        await using (var fileContent = await tusProvidedFile.GetContentAsync(cancellationToken))
        {
            if (fileContent is null)
            {
                _dbContext.Files.Remove(file);
                await _dbContext.SaveEntitiesAsync(cancellationToken);
                return new UploadNotFound();
            }

            var result = await _s3Uploader.PutFileStream(
                file.Id,
                file.FileSize,
                fileContent,
                cancellationToken);
            
            file.FileLocation = new S3FileLocation
            {
                Key = result.ObjectKey,
                BucketName = result.Bucket
            };
        }
        

        await _dbContext.SaveEntitiesAsync(cancellationToken);
        try
        {
            await _uploadManager.GetStore().DeleteFileAsync(tusFile.TusId, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Couldn't delete file");
        }

        return new UploadOk();
    }
}