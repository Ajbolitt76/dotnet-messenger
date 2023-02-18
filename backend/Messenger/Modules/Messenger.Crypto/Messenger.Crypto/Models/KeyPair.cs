namespace Messenger.Crypto.Models;

public class KeyPair
{
    private string? _publicKey;
    private string? _privateKey;

    /// <summary>
    /// Private key (base64) для подписи ссылок
    /// </summary>
    public string? PrivateKey
    {
        get => _privateKey;
        set
        {
            _privateKey = value;
            PrivateKeyBytes = value != null ? Convert.FromBase64String(value) : null;
        }
    }

    /// <summary>
    /// Public key (base64) для подписи ссылок
    /// </summary>
    public string? PublicKey
    {
        get => _publicKey;
        set
        {
            _publicKey = value;
            PublicKeyBytes = value != null ? Convert.FromBase64String(value) : null;
        }
    }

    /// <summary>
    /// Приватный ключ для подписи ссылок
    /// </summary>
    public byte[]? PrivateKeyBytes { get; private set; }

    /// <summary>
    /// Публичный ключ для подписи ссылок
    /// </summary>
    public byte[]? PublicKeyBytes { get; private set; }

}
