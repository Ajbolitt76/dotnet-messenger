namespace Messenger.Core.Services;

public interface IRedisKeyStore
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan expiry);
    Task SetAsync<T>(string key, T value);
    Task DeleteAsync<T>(string key);
    Task<bool> ExistsAsync<T>(string key);
    Task<long> IncrementAsync(string key);
    Task<IAsyncDisposable> LockAsync(string key, int maxWaitMs = 1000, TimeSpan? expiry = null);
}
