namespace Messenger.Core.Services;

public interface IRedisStore<T>
{
    Task<T?> GetAsync(string key);
    Task SetAsync(string key, T value, TimeSpan expiry);
    Task SetAsync(string key, T value);
    Task DeleteAsync(string key);
    Task<bool> ExistsAsync(string key);
}