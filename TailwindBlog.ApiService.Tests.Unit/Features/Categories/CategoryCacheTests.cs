# =======================================================
# Copyright (c) 2025. All rights reserved.
# File Name :     CategoryCacheTests.cs
# Company :       mpaulosky
# Author :        Matthew
# Solution Name : TailwindBlog
# Project Name :  TailwindBlog.ApiService.Tests.Unit
# =======================================================

using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TailwindBlog.ApiService.Features.Categories;
using TailwindBlog.Domain.Entities;
using Xunit;

namespace TailwindBlog.ApiService.Tests.Unit.Features.Categories;

[ExcludeFromCodeCoverage]
public class CategoryCacheTests
{
	private readonly IDistributedCache _cache;
	private readonly ILogger<CategoryCache> _logger;
	private readonly CategoryCache _categoryCache;

	public CategoryCacheTests()
	{
		_cache = Substitute.For<IDistributedCache>();
		_logger = Substitute.For<ILogger<CategoryCache>>();
		_categoryCache = new CategoryCache(_cache, _logger);
	}

	[Fact]
	public async Task GetAsync_ReturnsNull_WhenCacheItemDoesNotExist()
	{
		// Arrange
		var id = Guid.NewGuid();
		_cache.GetStringAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((string?)null);

		// Act
		var result = await _categoryCache.GetAsync(id);

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public async Task GetAsync_ReturnsCategory_WhenCacheItemExists()
	{
		// Arrange
		var id = Guid.NewGuid();
		var category = new Category("Test Category", "Test Description") { Id = id };
		var serializedCategory = JsonSerializer.Serialize(category);
		_cache.GetStringAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(serializedCategory);

		// Act
		var result = await _categoryCache.GetAsync(id);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(id);
		result.Name.Should().Be("Test Category");
		result.Description.Should().Be("Test Description");
	}

	[Fact]
	public async Task SetAsync_SetsItemInCache()
	{
		// Arrange
		var category = new Category("Test Category", "Test Description");

		// Act
		await _categoryCache.SetAsync(category);

		// Assert
		await _cache.Received(1).SetStringAsync(
				Arg.Is<string>(key => key == $"category-{category.Id}"),
				Arg.Any<string>(),
				Arg.Any<DistributedCacheEntryOptions>(),
				Arg.Any<CancellationToken>()
		);
	}

	[Fact]
	public async Task RemoveAsync_RemovesItemFromCache()
	{
		// Arrange
		var id = Guid.NewGuid();

		// Act
		await _categoryCache.RemoveAsync(id);

		// Assert
		await _cache.Received(1).RemoveAsync(
				Arg.Is<string>(key => key == $"category-{id}"),
				Arg.Any<CancellationToken>()
		);
	}
}
