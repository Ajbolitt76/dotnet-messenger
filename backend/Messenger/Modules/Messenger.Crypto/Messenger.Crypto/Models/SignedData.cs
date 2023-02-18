namespace Messenger.Crypto.Models;

/// <summary>
/// Обертка с подписью объекта
/// </summary>
/// <param name="Data">Подписанные данные</param>
/// <param name="Signature">Base64 подпись</param>
/// <typeparam name="T"></typeparam>
public record SignedData<T>(T Data, string Signature); 
