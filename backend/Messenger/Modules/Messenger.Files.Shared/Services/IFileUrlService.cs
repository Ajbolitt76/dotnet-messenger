using Messenger.Crypto.Models;
using Messenger.Files.Shared.FileRequests;

namespace Messenger.Files.Shared.Services;

public interface IFileUrlService
{
    public SignedData<FileRequest> GetSignedFileRequest(Guid fileId);

    public SignedData<T> GetSignedFileRequest<T>(T baseRequest) where T : FileRequest;

    public string GetSignedFileRequestQuery<T>(T baseRequest) where T : FileRequest;
}
