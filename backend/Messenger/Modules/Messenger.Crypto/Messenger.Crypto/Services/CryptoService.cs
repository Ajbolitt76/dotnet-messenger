using System.Text.Json;
using Microsoft.Extensions.Options;
using NSec.Cryptography;
using Messenger.Crypto.Models;

namespace Messenger.Crypto.Services;

public class CryptoService : ICryptoService
{
    private readonly SignatureAlgorithm _signatureAlgorithm = SignatureAlgorithm.Ed25519;

    private static readonly AeadAlgorithm _aeadAlgorithm = AeadAlgorithm.XChaCha20Poly1305;

    private readonly KeyAgreementAlgorithm _keyAgreement = KeyAgreementAlgorithm.X25519;

    private readonly KeyDerivationAlgorithm _kdf = KeyDerivationAlgorithm.HkdfSha256;

    private readonly Random _random = new();

    private readonly string _defaultInfo = "Messenger";

    public readonly int _publicDataLength = KeyAgreementAlgorithm.X25519.PublicKeySize + _aeadAlgorithm.NonceSize;
    private readonly JsonSerializerOptions _jsonOptions;

    public CryptoService(IOptions<JsonSerializerOptions> options)
    {
        _jsonOptions = options.Value;
    }

    public SignedData<T> SignObject<T>(T data, byte[] privateKey)
    {
        var dataBytes = JsonSerializer.SerializeToUtf8Bytes(data, _jsonOptions);
        var base64Data = Convert.ToBase64String(Sign(dataBytes, privateKey));
        return new SignedData<T>(data, base64Data);
    }
    
    public bool VerifyObject<T>(SignedData<T> signedData, ReadOnlySpan<byte> publicKey)
    {
        var dataBytes = JsonSerializer.SerializeToUtf8Bytes(signedData.Data, _jsonOptions);
        var signature = Convert.FromBase64String(signedData.Signature);
        return Verify(dataBytes, signature, publicKey);
    }

    public byte[] Sign(ReadOnlySpan<byte> data, ReadOnlySpan<byte> privateKey)
    {
        var output = new byte[_signatureAlgorithm.SignatureSize];
        using var key = Key.Import(_signatureAlgorithm, privateKey, KeyBlobFormat.RawPrivateKey);
        _signatureAlgorithm.Sign(key, data, output);
        return output;
    }

    public bool Verify(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> publicKey)
    {
        var key = PublicKey.Import(_signatureAlgorithm, publicKey, KeyBlobFormat.RawPublicKey);
        return _signatureAlgorithm.Verify(key, data, signature);
    }

    public EncryptionResult AsymmetricEncrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> publicKey)
    {
        var key = PublicKey.Import(_keyAgreement, publicKey, KeyBlobFormat.RawPublicKey);
        using var ephemeralKey = Key.Create(_keyAgreement);
        var ephemeralPublicKey = ephemeralKey.Export(KeyBlobFormat.RawPublicKey);

        using var sharedSecret = _keyAgreement.Agree(ephemeralKey, key)
            ?? throw new InvalidOperationException("Не удалось создать общий секрет");

        var nonce = new byte[_aeadAlgorithm.NonceSize];
        _random.NextBytes(nonce);
        
        using var symmetricKey = DeriveKey(sharedSecret, _aeadAlgorithm, "generated"u8, nonce);

        var s = Convert.ToBase64String(sharedSecret.Export(SharedSecretBlobFormat.RawSharedSecret));
        var ss = Convert.ToBase64String(symmetricKey.Export(KeyBlobFormat.RawSymmetricKey));
        
        var publicData = new byte[_publicDataLength];
        ephemeralPublicKey.CopyTo(publicData, 0);
        nonce.CopyTo(publicData, ephemeralPublicKey.Length);

        return new EncryptionResult
        {
            EncryptedData = _aeadAlgorithm.Encrypt(symmetricKey, nonce, publicData, data),
            Nonce = nonce,
            PublicKey = ephemeralPublicKey
        };
    }

    public byte[]? AsymmetricDecrypt(EncryptionResult data, ReadOnlySpan<byte> privateKey)
    {
        using var key = Key.Import(_keyAgreement, privateKey, KeyBlobFormat.RawPrivateKey);
        var ephemeralPublicKey = PublicKey.Import(_keyAgreement, data.PublicKey, KeyBlobFormat.RawPublicKey);

        using var sharedSecret = _keyAgreement.Agree(key, ephemeralPublicKey,  new()
            {
                ExportPolicy = KeyExportPolicies.AllowPlaintextExport
            })
            ?? throw new InvalidOperationException("Не удалось создать общий секрет");

        using var symmetricKey = DeriveKey(sharedSecret, _aeadAlgorithm, "generated"u8, data.Nonce);

        var publicData = new byte[_publicDataLength];
        data.PublicKey.CopyTo(publicData, 0);
        data.Nonce.CopyTo(publicData, data.PublicKey.Length);
        
        return _aeadAlgorithm.Decrypt(symmetricKey, data.Nonce, publicData, data.EncryptedData);
    }

    public byte[] SymmetricEncrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce)
    {
        using var encryptionKey = Key.Import(_aeadAlgorithm, key, KeyBlobFormat.RawPrivateKey);
        return _aeadAlgorithm.Encrypt(encryptionKey, nonce, "gp"u8, data);
    }

    public byte[]? SymmetricDecrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce)
    {
        using var encryptionKey = Key.Import(_aeadAlgorithm, key, KeyBlobFormat.RawPrivateKey);
        return _aeadAlgorithm.Encrypt(encryptionKey, nonce, "gp"u8, data);
    }

    public Key DeriveKey(SharedSecret sharedSecret, Algorithm algorithm, ReadOnlySpan<byte> info, byte[]? salt = null)
    {
        if (salt == null)
        {
            salt = new byte[16];
            _random.NextBytes(salt);
        }

        return _kdf.DeriveKey(sharedSecret, salt, info, algorithm);
    }
}
