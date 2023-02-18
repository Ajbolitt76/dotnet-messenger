using Microsoft.Extensions.Options;
using Messenger.Crypto.Models;

namespace Messenger.Crypto.Services;

public class ModuleSignatureValidator<TKeyId>
{
    private readonly ICryptoService _cryptoService;
    private readonly byte[]? _publicKey;

    public ModuleSignatureValidator(ICryptoService cryptoService, byte[]? publicKey)
    {
        _cryptoService = cryptoService;
        _publicKey = publicKey;
    }

    public bool Validate<T>(SignedData<T> signedData)
        => _publicKey == null || _cryptoService.VerifyObject(signedData, _publicKey);
}
