namespace Messenger.Core;

/// <summary>
/// Задает интерфейс для класса, который имеет дискриминатор и может быть преобразован сериализован.
/// </summary>
public interface IHaveDiscriminator
{
    public static abstract string Discriminator { get; }
}
