// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     CategoryCachingTests.cs
// Company:       mpaulosky
// Author:        Matthew
// Solution Name: TailwindBlog
// Project Name:  TailwindBlog.ApiService.Tests.Integration
// =======================================================

using Microsoft.Extensions.Caching.Distributed;
using TailwindBlog.Domain.Entities;

namespace TailwindBlog.ApiService.Tests.Integration.Features.Categories;

public class CategoryCachingTests : ApiTestBase
{
	private readonly IDistributedCache _cache;

	public CategoryCachingTests(ApiAppFactory factory) : base(factory)
	{
		_cache = factory.Services.GetRequiredService<IDistributedCache>();
	}

	[Fact]
	public async Task GetCategory_ShouldCacheResult()
	{
		// Arrange
		var category = new Category("Test Category", "Test Description");
		await AddAsync(category);

		// Act
		var response1 = await HttpClient.GetAsync($"/api/categories/{category.Id}");
		var response2 = await HttpClient.GetAsync($"/api/categories/{category.Id}");

		// Assert
		response1.EnsureSuccessStatusCode();
		response2.EnsureSuccessStatusCode();

		// The second request should be served from cache
		var cacheKey = $"category-{category.Id}";
		var cachedValue = await _cache.GetStringAsync(cacheKey);
		cachedValue.Should().NotBeNull("The category should be cached");
	}

	[Fact]
	public async Task UpdateCategory_ShouldInvalidateCache()
	{
		// Arrange
		var category = new Category("Initial Name", "Initial Description");
		await AddAsync(category);

		// Act - First get to cache it
		await HttpClient.GetAsync($"/api/categories/{category.Id}");

		// Update the category
		var updateRequest = new UpdateCategoryCommand
		{
			Id = category.Id.ToString(),
			Name = "Updated Name",
			Description = "Updated Description"
		};

		var updateResponse = await HttpClient.PutAsJsonAsync($"/api/categories/{category.Id}", updateRequest);
		updateResponse.EnsureSuccessStatusCode();

		// Assert
		var cacheKey = $"category-{category.Id}";
		var cachedValue = await _cache.GetStringAsync(cacheKey);
		cachedValue.Should().BeNull("Cache should be invalidated after update");
	}

	[Fact]
	public async Task DeleteCategory_ShouldInvalidateCache()
	{
		// Arrange
		var category = new Category("Test Category", "Test Description");
		await AddAsync(category);

		// Act - First get to cache it
		await HttpClient.GetAsync($"/api/categories/{category.Id}");

		// Delete the category
		var deleteResponse = await HttpClient.DeleteAsync($"/api/categories/{category.Id}");
		deleteResponse.EnsureSuccessStatusCode();

		// Assert
		var cacheKey = $"category-{category.Id}";
		var cachedValue = await _cache.GetStringAsync(cacheKey);
		cachedValue.Should().BeNull("Cache should be invalidated after delete");
	}
}
