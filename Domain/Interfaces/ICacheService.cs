// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ICacheService.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =======================================================

namespace Persistence.Services;

/// <summary>
/// Abstraction for asynchronous cache operations.
/// </summary>
public interface ICacheService
{

	/// <summary>
	/// Asynchronously retrieves a cached item by key.
	/// </summary>
	/// <typeparam name="TValue">Type of the value to retrieve.</typeparam>
	/// <param name="key">The cache key.</param>
	/// <returns>The cached item or default if not found.</returns>
	Task<TValue?> GetAsync<TValue>(string key);

	/// <summary>
	/// Asynchronously sets a value in the cache with the given key and expiration.
	/// </summary>
	/// <typeparam name="TValue">Type of the value to cache.</typeparam>
	/// <param name="key">The cache key.</param>
	/// <param name="value">The value to cache.</param>
	/// <param name="expiration">The relative expiration time from now.</param>
	Task SetAsync<TValue>(string key, TValue value, TimeSpan expiration);

	/// <summary>
	/// Asynchronously removes the cached value by key.
	/// </summary>
	/// <param name="key">The cache key.</param>
	Task RemoveAsync(string key);

}