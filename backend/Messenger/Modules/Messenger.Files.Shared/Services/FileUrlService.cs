using Microsoft.Extensions.Options;
using Messenger.Crypto.Models;
using Messenger.Crypto.Services;
using Messenger.Files.Shared.Extensions;
using Messenger.Files.Shared.FileRequests;

namespace Messenger.Files.Shared.Services;

public class FileUrlService : IFileUrlService
{
    private readonly ICryptoService _cryptoService;
    private readonly FileModuleOptions _fileModuleOptions;

    public FileUrlService(ICryptoService cryptoService, IOptions<FileModuleOptions> options)
    {
        _cryptoService = cryptoService;
        _fileModuleOptions = options.Value;
    }

    public SignedData<FileRequest> GetSignedFileRequest(Guid fileId)
    {
        var fileRequest = new FileRequest
        {
            FileId = fileId
        };

        if (_fileModuleOptions.SignLinks)
            throw new InvalidOperationException("Подпись ссылок не включена");

        var signedData = _cryptoService.SignObject(fileRequest, _fileModuleOptions.LinkSigningKeyPair.PrivateKeyBytes!);

        return signedData;
    }

    public SignedData<T> GetSignedFileRequest<T>(T baseRequest) where T : FileRequest
        => _cryptoService.SignObject(baseRequest, _fileModuleOptions.LinkSigningKeyPair.PrivateKeyBytes!);

    public string GetSignedFileRequestQuery<T>(T baseRequest) where T : FileRequest
        => GetSignedFileRequest(baseRequest).ToQuery();
}
