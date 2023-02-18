namespace Messenger.Core.Model.FileAggregate.FileLocation;

public class S3FileLocation : IFileLocation, IHaveJsonDiscriminator
{
    public static string Discriminator => "s3";
    
    public string BucketName { get; set; }
    
    public string Key { get; set; }
}
