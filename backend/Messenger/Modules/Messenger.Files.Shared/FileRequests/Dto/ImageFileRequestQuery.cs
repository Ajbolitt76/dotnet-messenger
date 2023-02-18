namespace Messenger.Files.Shared.FileRequests.Dto;

public class ImageFileRequestQuery
{
    public Guid FileId { get; set; }

    public long? Expiry { get; set; }
    
    public string? Type { get; set; }
    
    public int? Height { get; set; }
    
    public int? Width { get; set; }
    
    public ImageFileRequest ToEntity()
    {
        return new ImageFileRequest
        {
            FileId = FileId,
            Expiry = Expiry != null ? DateTimeOffset.FromUnixTimeSeconds(Expiry.Value).UtcDateTime : null,
            Type = Type,
            Height = Height,
            Width = Width
        };
    }
}
