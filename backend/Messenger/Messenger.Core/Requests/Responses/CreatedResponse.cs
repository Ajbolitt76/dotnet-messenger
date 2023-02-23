namespace Messenger.Core.Requests.Responses;

/// <summary>
/// Ответ о созданном объекте
/// </summary>
/// <param name="Created">Создан или уже существовал</param>
/// <param name="Id">ID созданного объекта</param>
/// <typeparam name="T">Тип ID</typeparam>
public record CreatedResponse<T>(bool Created, T? Id);
