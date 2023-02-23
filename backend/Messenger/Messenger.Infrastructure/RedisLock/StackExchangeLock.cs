using StackExchange.Redis;

namespace Messenger.Infrastructure.RedisLock;

public class StackExchangeLock : IDisposable, IAsyncDisposable
{
    private readonly IDatabase _database;
    private readonly string _key;
    private readonly string _token;
    private readonly bool _acquired;

    public StackExchangeLock(IDatabase database, string key, string token, bool acquired)
    {
        _database = database;
        _key = key;
        _token = token;
        _acquired = acquired;
    }
    
    public void Dispose()
    {
        _database.LockRelease(_key, _token);
    }

    public async ValueTask DisposeAsync()
    {
        await _database.LockReleaseAsync(_key, _token);
    }
}
