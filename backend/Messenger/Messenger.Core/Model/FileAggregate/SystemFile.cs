using Messenger.Core.Model.FileAggregate.FileLocation;

namespace Messenger.Core.Model.FileAggregate;

public class SystemFile : BaseFileInfo
{
    public IFileLocation FileLocation { get; set; }

    public SystemFile(string fileName, long fileSize, IFileLocation fileLocation) : base(fileName, fileSize)
    {
        FileLocation = fileLocation;
    }

    public SystemFile(BaseFileInfo fileInfo, IFileLocation fileLocation) : base(fileInfo.FileName, fileInfo.FileSize)
    {
        CreatorIp = fileInfo.CreatorIp;
        CreatedById = fileInfo.CreatedById;
        
        FileLocation = fileLocation;
    }
}
