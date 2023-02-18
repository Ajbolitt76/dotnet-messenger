namespace Messenger.Files.Shared.FileRequests;

public class ImageFileRequest : FileRequests.FileRequest
{
    public int? Height { get; set; }
    
    public int? Width { get; set; }

    public override Uri ToQuery()
    {
        var ub = new UriBuilder(base.ToQuery());
        
        ub.Query += $"&{nameof(Width)}={Width}&{nameof(Height)}={Height}";
        ub.Path += "picture/";
        
        return ub.Uri;
    }
}
