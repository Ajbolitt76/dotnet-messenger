namespace Messenger.S3.Models;

public record UploadResult(
    string Bucket, 
    string ObjectKey);