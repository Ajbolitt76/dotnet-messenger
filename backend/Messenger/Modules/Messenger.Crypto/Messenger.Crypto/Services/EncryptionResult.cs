namespace Messenger.Crypto.Services;

public struct EncryptionResult
{
    public byte[] EncryptedData { get; set; }
    public byte[] PublicKey { get; set; }
    public byte[] Nonce { get; set; }
}
