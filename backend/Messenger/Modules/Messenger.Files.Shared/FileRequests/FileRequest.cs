using System.Text;

namespace Messenger.Files.Shared.FileRequests;

public class FileRequest
{
    private DateTime? _expiry;
    public Guid FileId { get; set; }

    public DateTime? Expiry
    {
        get => _expiry;
        set
        {
            if(value != null && value.Value.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Expiry must be UTC", nameof(Expiry));
            _expiry = value;
        }
    }

    public string? Type { get; set; }

    public virtual Uri ToQuery()
    {
        var ub = new UriBuilder();
        var sb = new StringBuilder();
        
        sb.AppendFormat("{0}={1}", nameof(FileId), FileId.ToString());

        if (Expiry != null)
            sb.AppendFormat("&{0}={1}", nameof(Expiry), new DateTimeOffset(Expiry.Value).ToUnixTimeSeconds());

        if(Type != null)
            sb.AppendFormat("&{0}={1}", nameof(Type), Type);
        
        ub.Query = sb.ToString();
        return ub.Uri;
    }
}
