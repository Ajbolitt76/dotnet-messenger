namespace Messenger.Core.Services;

public interface IDateTimeProvider
{
    public DateTime NowUtc { get; }
}