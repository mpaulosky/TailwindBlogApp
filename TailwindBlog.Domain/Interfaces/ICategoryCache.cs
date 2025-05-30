// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ICategoryCache.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Interfaces;

/// <summary>
///   Defines caching operations for categories.
/// </summary>
public interface ICategoryCache
{

	/// <summary>
	///   Gets a category from cache by its ID.
	/// </summary>
	/// <param name="id">The category ID.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>The cached category, or null if not found.</returns>
	Task<Category?> GetAsync(ObjectId id, CancellationToken cancellationToken = default);

	/// <summary>
	///   Sets a category in the cache.
	/// </summary>
	/// <param name="category">The category to cache.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	Task SetAsync(Category category, CancellationToken cancellationToken = default);

	/// <summary>
	///   Removes a category from the cache.
	/// </summary>
	/// <param name="id">The category ID.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	Task RemoveAsync(ObjectId id, CancellationToken cancellationToken = default);

}