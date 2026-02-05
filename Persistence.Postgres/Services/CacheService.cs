using Microsoft.Extensions.Caching.Memory;

namespace Persistence.Postgres.Services;

public class CacheService : ICacheService
{

	private readonly IMemoryCache _memoryCache;

	public CacheService(IMemoryCache memoryCache)
	{
		_memoryCache = memoryCache;
	}

	public Task<TValue?> GetAsync<TValue>(string key)
	{
		return Task.FromResult(_memoryCache.TryGetValue(key, out var value) ? (TValue?)value : default);
	}

	public Task SetAsync<TValue>(string key, TValue value, TimeSpan expiration)
	{
		_memoryCache.Set(key, value, expiration);

		return Task.CompletedTask;
	}

	public Task RemoveAsync(string key)
	{
		_memoryCache.Remove(key);

		return Task.CompletedTask;
	}

}