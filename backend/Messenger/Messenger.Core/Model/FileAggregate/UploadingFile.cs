using Messenger.Core.Model.UserAggregate;

namespace Messenger.Core.Model.FileAggregate;

public class UploadingFile : BaseFileInfo
{
    public bool IsFinished { get; set; }
    
    public string TusId { get; set; }

    public UploadingFile(string tusId, string fileName, long fileSize) : base(fileName, fileSize)
    {
        TusId = tusId;
    }
}
