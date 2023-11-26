using Microsoft.Extensions.Options;
using Messenger.Crypto.Models;

namespace Messenger.Crypto.Services;

public class ModuleSignatureService<TKeyId>
{
    private readonly ICryptoService _cryptoService;
    private readonly byte[]? _publicKey;
    private readonly byte[]? _privateKey;

    public ModuleSignatureService(
        ICryptoService cryptoService,
        byte[]? publicKey,
        byte[]? privateKey)
    {
        _cryptoService = cryptoService;
        _publicKey = publicKey;
        _privateKey = privateKey;
    }

    public bool Validate<T>(SignedData<T> signedData)
        => _publicKey == null || _cryptoService.VerifyObject(signedData, _publicKey);

    public SignedData<T> Sign<T>(T toSign)
    {
        if (_privateKey is null)
        {
            throw new InvalidOperationException("Can't sign because no private key was specified");
        }
        var signed = _cryptoService.SignObject(toSign, _privateKey);
        var isValid = Validate<T>(signed);
        return _cryptoService.SignObject(toSign, _privateKey);
    }
}
