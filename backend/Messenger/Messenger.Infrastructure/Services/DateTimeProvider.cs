using Messenger.Core.Services;

namespace Messenger.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime NowUtc => DateTime.UtcNow;
}