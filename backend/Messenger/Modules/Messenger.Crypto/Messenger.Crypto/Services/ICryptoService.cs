using Messenger.Crypto.Models;

namespace Messenger.Crypto.Services;

public interface ICryptoService
{
    public SignedData<T> SignObject<T>(T data, byte[] privateKey);

    public bool VerifyObject<T>(SignedData<T> signedData, ReadOnlySpan<byte> publicKey);

    public byte[] Sign(ReadOnlySpan<byte> data, ReadOnlySpan<byte> privateKey);
    
    public bool Verify(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> publicKey);
    
    public EncryptionResult AsymmetricEncrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> publicKey);
    
    public byte[]? AsymmetricDecrypt(EncryptionResult data, ReadOnlySpan<byte> privateKey);
    
    public byte[] SymmetricEncrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv);
    
    public byte[]? SymmetricDecrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv);
}
