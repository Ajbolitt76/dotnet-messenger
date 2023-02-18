using FluentValidation;
using Messenger.Crypto.Models;
using Messenger.Infrastructure.Validation;

namespace Messenger.Files;

public class FileModuleOptions
{
    private string _publicKey;
    private string _privateKey;

    /// <summary>
    /// Хранилище файлов
    /// </summary>
    public string FileStoragePath { get; set; }

    /// <summary>
    /// Время жизни файла в секундах без продления (скользящее окно)
    /// </summary>
    public int Expiration { get; set; } = 60 * 10;

    /// <summary>
    /// Максимальный размер файла в байтах
    /// </summary>
    public long? MaxAllowedUploadSizeInBytesLong { get; set; } = 1024 * 1024 * 100;

    /// <summary>
    /// Ключ названия файла в метаданных
    /// </summary>
    public string FileNameMetadataKey { get; set; } = "filename";

    /// <summary>
    /// Включить подпись ссылок
    /// </summary>
    public bool SignLinks { get; set; }
    
    /// <summary>
    /// Включть верификацию подписи ссылок
    /// </summary>
    public bool VerifyLinks { get; set; }

    /// <summary>
    /// Ключ подписи ссылок
    /// </summary>
    public KeyPair LinkSigningKeyPair { get; set; }

    [DoNotAutoRegister]
    public class FileModuleOptionsValidator : AbstractValidator<FileModuleOptions>
    {
        public FileModuleOptionsValidator()
        {
            RuleFor(x => x.FileStoragePath).NotEmpty();
            RuleFor(x => x.Expiration).GreaterThan(0);
            RuleFor(x => x.MaxAllowedUploadSizeInBytesLong).GreaterThanOrEqualTo(0);

            When(
                x => x.SignLinks,
                () =>
                {
                    RuleFor(x => x.LinkSigningKeyPair).NotNull()
                        .WithMessage("Должны быть указаны ключи подписи ссылок, если включена подпись ссылок");
                    RuleFor(x => x.LinkSigningKeyPair.PrivateKey).NotEmpty()
                        .WithMessage("Должен быть указан приватный ключ подписи ссылок, если включена подпись ссылок");
                    RuleFor(x => x.LinkSigningKeyPair.PublicKey).NotEmpty()
                        .WithMessage("Должен быть указан публичный ключ подписи ссылок, если включена подпись ссылок");
                });
        }
    }
}
