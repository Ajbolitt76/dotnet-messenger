namespace Messenger.Core;

/// <summary>
/// Задает интерфейс для класса, который имеет дискриминатор и может быть преобразован в JSON.
/// </summary>
public interface IHaveJsonDiscriminator
{
    public static abstract string Discriminator { get; }
}
