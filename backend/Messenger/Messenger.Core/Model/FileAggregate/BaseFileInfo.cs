using System.Net;
using Messenger.Core.Model.UserAggregate;

namespace Messenger.Core.Model.FileAggregate;

public class BaseFileInfo : BaseEntity
{
    public string FileName { get; set; }
    
    public long FileSize { get; set; }

    public Guid? CreatedById { get; set; }
    
    public IPAddress? CreatorIp { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public BaseFileInfo(string fileName, long fileSize)
    {
        FileName = fileName;
        FileSize = fileSize;
    }
    
    #region Navigation Properties
    
    public RepetUser? CreatedByUser { get; set; }

    #endregion
}
