namespace Messenger.Core.Model.FileAggregate.FileLocation;

public class TusFileLocation : IFileLocation, IHaveJsonDiscriminator
{
    public static string Discriminator => "tus";
    
    public string TusId { get; set; }

    public TusFileLocation(string tusId)
    {
        TusId = tusId;
    }
}
