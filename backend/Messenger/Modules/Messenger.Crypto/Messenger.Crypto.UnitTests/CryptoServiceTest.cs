using System.Text.Json;
using Microsoft.Extensions.Options;
using NSec.Cryptography;
using Messenger.Crypto.Services;

namespace Messenger.Crypto.UnitTests;

public class CryptoServiceTest
{
    private readonly ICryptoService _cryptoService =
        new CryptoService(new OptionsWrapper<JsonSerializerOptions>(JsonSerializerOptions.Default));

    private readonly byte[] TestPublicKey = Convert.FromBase64String("C/VAKwN4Hlxua5Jvg/dh8t5ckbVohOauTaUie0Qcrts=");
    private readonly byte[] TestPrivateKey = Convert.FromBase64String("khKKxiYMC4kerq23pvgBFHfpTHX2tRisTDFS6UbX4IQ=");

    [Fact]
    public void Should_SignatureBeValidatable_When_EverethyngIsCorrect()
    {
        var dataToSign = "Hello World"u8;

        var signature = _cryptoService.Sign(dataToSign, TestPrivateKey);
        Assert.True(_cryptoService.Verify(dataToSign, signature, TestPublicKey));
    }

    [Fact]
    public void Should_SignatureBeInvalid_When_DataIsChanged()
    {
        var dataToSign = "Hello World"u8;
        var dataToSignChanged = "Hello World!"u8;

        var signature = _cryptoService.Sign(dataToSign, TestPrivateKey);
        Assert.False(_cryptoService.Verify(dataToSignChanged, signature, TestPublicKey));
    }

    [Fact]
    public void Should_AsymmetricEncryptionBeValid_When_EverythingIsCorrect()
    {
        var dataToEncrypt = "Hello World"u8;

        var x25519KeyPair = Key.Create(
            KeyAgreementAlgorithm.X25519,
            new()
            {
                ExportPolicy = KeyExportPolicies.AllowPlaintextExport
            });

        var encryptedData = _cryptoService.AsymmetricEncrypt(
            dataToEncrypt,
            x25519KeyPair.Export(KeyBlobFormat.RawPublicKey));
        var decryptedData = _cryptoService.AsymmetricDecrypt(
            encryptedData,
            x25519KeyPair.Export(KeyBlobFormat.RawPrivateKey));

        Assert.Equal(dataToEncrypt.ToArray(), decryptedData);
    }

    [Fact]
    public void Should_AsymmetricEncryptionBeInvalid_When_DataIsChanged()
    {
        var dataToEncrypt = "Hello World"u8;

        var encryptedData = _cryptoService.AsymmetricEncrypt(dataToEncrypt, TestPublicKey);

        encryptedData.EncryptedData[3] += 1;

        var decryptedData = _cryptoService.AsymmetricDecrypt(encryptedData, TestPrivateKey);

        Assert.Null(decryptedData);
    }
}
