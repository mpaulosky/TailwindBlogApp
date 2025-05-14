// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     CategoryCache.cs
// Company:       mpaulosky
// Author:        Matthew
// Solution Name: TailwindBlog
// Project Name:  TailwindBlog.ApiService
// =======================================================

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using TailwindBlog.Domain.Entities;
using TailwindBlog.Domain.Interfaces;

namespace TailwindBlog.ApiService.Features.Categories;

/// <summary>
/// Implements caching operations for categories using distributed cache.
/// </summary>
public class CategoryCache : ICategoryCache
{
	private readonly IDistributedCache _cache;
	private readonly ILogger<CategoryCache> _logger;
	private readonly DistributedCacheEntryOptions _cacheOptions;

	private static string GetCacheKey(Guid id) => $"category-{id}";

	/// <summary>
	/// Initializes a new instance of the <see cref="CategoryCache"/> class.
	/// </summary>
	public CategoryCache(IDistributedCache cache, ILogger<CategoryCache> logger)
	{
		_cache = cache;
		_logger = logger;
		_cacheOptions = new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
			SlidingExpiration = TimeSpan.FromMinutes(30)
		};
	}

	/// <inheritdoc />
	public async Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default)
	{
		try
		{
			var cacheKey = GetCacheKey(id);
			var serializedValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (string.IsNullOrEmpty(serializedValue))
			{
				_logger.LogDebug("Category {CategoryId} not found in cache", id);
				return null;
			}

			var category = JsonSerializer.Deserialize<Category>(serializedValue);
			_logger.LogDebug("Category {CategoryId} retrieved from cache", id);
			return category;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error getting category {CategoryId} from cache", id);
			return null;
		}
	}

	/// <inheritdoc />
	public async Task SetAsync(Category category, CancellationToken cancellationToken = default)
	{
		try
		{
			var cacheKey = GetCacheKey(category.Id);
			var serializedValue = JsonSerializer.Serialize(category);

			await _cache.SetStringAsync(cacheKey, serializedValue, _cacheOptions, cancellationToken);
			_logger.LogDebug("Category {CategoryId} cached successfully", category.Id);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error caching category {CategoryId}", category.Id);
		}
	}

	/// <inheritdoc />
	public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
	{
		try
		{
			var cacheKey = GetCacheKey(id);
			await _cache.RemoveAsync(cacheKey, cancellationToken);
			_logger.LogDebug("Category {CategoryId} removed from cache", id);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error removing category {CategoryId} from cache", id);
		}
	}
}
