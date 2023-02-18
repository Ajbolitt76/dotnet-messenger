namespace Messenger.S3;

public class S3Options
{
    public string ServiceUrl { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string BucketName { get; set; }
    public string? Region { get; set; }
}
