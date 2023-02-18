namespace Messenger.Files.Shared.FileRequests.Dto;

public class FileRequestQuery
{
    public Guid FileId { get; set; }

    public long? Expiry { get; set; }
    
    public string? Type { get; set; }
    
    public FileRequest ToEntity()
    {
        return new FileRequest
        {
            FileId = FileId,
            Expiry = Expiry != null ? DateTimeOffset.FromUnixTimeSeconds(Expiry.Value).UtcDateTime : null,
            Type = Type
        };
    }
}
