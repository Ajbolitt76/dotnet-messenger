using Messenger.Core.Services;
using Messenger.Infrastructure.RedisLock;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Messenger.Infrastructure.Services;

public class RedisKeyStore : IRedisKeyStore
{
    private readonly IRedisDatabase _redisDatabase;

    public RedisKeyStore(IRedisClient redisClient)
    {
        _redisDatabase = redisClient.Db1;
    }

    public Task<T?> GetAsync<T>(string key)
        => _redisDatabase.GetAsync<T>(key);

    public Task SetAsync<T>(string key, T value, TimeSpan expiry)
        => _redisDatabase.AddAsync<T>(key, value, expiry);

    public Task SetAsync<T>(string key, T value)
        => _redisDatabase.AddAsync<T>(key, value);

    public Task DeleteAsync<T>(string key)
        => _redisDatabase.RemoveAsync(key);

    public Task<bool> ExistsAsync<T>(string key)
        => _redisDatabase.ExistsAsync(key);

    public Task<long> IncrementAsync(string key)
        => _redisDatabase.Database.StringIncrementAsync(key, 1L);
    
    public async Task<IAsyncDisposable> LockAsync(string key, int maxWaitMs = 1000, TimeSpan? expiry = null)
    {
        var token = Guid.NewGuid().ToString();
        var pause = Math.Max(maxWaitMs / 10, 40);
        for (int i = 0; i < 10; i++)
        {
            if (await _redisDatabase.Database.LockTakeAsync(key, token, expiry ?? TimeSpan.FromMinutes(2)))
                return new StackExchangeLock(_redisDatabase.Database, key, token, true);
            await Task.Delay(pause);
        }

        return new StackExchangeLock(_redisDatabase.Database, key, token, true);
    }
}
