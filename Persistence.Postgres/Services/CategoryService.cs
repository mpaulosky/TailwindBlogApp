// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CategoryService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =============================================

using Domain.Abstractions;
using Domain.Models;

using Mapster;

using MongoDB.Bson;

namespace Persistence.Postgres.Services;

/// <summary>
///   CategoryService class
/// </summary>
public class CategoryService : ICategoryService
{

	private const string _cacheName = "CategoryData";

	private readonly ICacheService _cache;

	private readonly ICategoryRepository _repository;

	/// <summary>
	///   CategoryService constructor
	/// </summary>
	/// <param name="repository">ICategoryRepository</param>
	/// <param name="cache">IMemoryCache</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CategoryService(ICategoryRepository repository, ICacheService cache)
	{
		ArgumentNullException.ThrowIfNull(repository);
		ArgumentNullException.ThrowIfNull(cache);

		_repository = repository;
		_cache = cache;
	}

	/// <summary>
	///   Archives a given category.
	/// </summary>
	/// <param name="category">The category to be archived. Must not be null.</param>
	/// <returns>
	///   A <see cref="Result" /> indicating the success or failure of the operation. If the operation fails,
	///   the result contains an error message.
	/// </returns>
	public async Task<Result> ArchiveAsync(CategoryDto? category)
	{

		// Validate input
		if (category == null)
		{
			return Result.Fail("Category cannot be null.");
		}

		// Remove the cache for categories so it's refreshed after the change
		await _cache.RemoveAsync(_cacheName);

		try
		{

			await _repository.ArchiveAsync(category.Adapt<Category>());

			return Result.Ok();

		}
		catch (Exception ex)
		{

			return Result.Fail($"Failed to archive category: {ex.Message}");

		}

	}

	/// <summary>
	///   Asynchronously creates a new category.
	/// </summary>
	/// <param name="category">The category data transfer object to be created.</param>
	/// <returns>
	///   A <see cref="Result" /> indicating the success or failure of the operation. If the operation fails,
	///   the result contains an error message.
	/// </returns>
	public async Task<Result> CreateAsync(CategoryDto? category)
	{

		// Validate input
		if (category == null)
		{
			return Result.Fail("Category cannot be null.");
		}

		// Remove the cache so it's refreshed on next GetAll
		await _cache.RemoveAsync(_cacheName);

		try
		{

			// Call a repository to create
			await _repository.CreateAsync(category.Adapt<Category>());

			return Result.Ok();

		}
		catch (Exception ex)
		{

			// Handle or log exception as necessary
			return Result.Fail($"Failed to create category: {ex.Message}");

		}

	}

	/// <summary>
	///   Retrieves a category by its unique identifier.
	/// </summary>
	/// <param name="categoryId">The unique identifier of the category to retrieve.</param>
	/// <returns>
	///   A <see cref="Result{T}" /> containing a <see cref="CategoryDto" /> if successful, or a failure result with an
	///   error message.
	/// </returns>
	public async Task<Result<CategoryDto>> GetAsync(ObjectId categoryId)
	{

		// Validate input
		if (categoryId == ObjectId.Empty)
		{
			return Result<CategoryDto>.Fail("Category id cannot be empty.");
		}

		// Try to get all categories from the cache
		var categoryList = await _cache.GetAsync<List<CategoryDto>>(_cacheName);

		// If found in the cache, try to return the matching category
		var cachedCategory = categoryList?.FirstOrDefault(c => c.Id == categoryId);

		if (cachedCategory != null)
		{
			return Result.Ok(cachedCategory);
		}

		// Not cached, get from repository
		var category = await _repository.GetAsync(categoryId);

		if (category.Failure)
		{
			return Result<CategoryDto>.Fail("Category not found.");
		}

		// Adapt model to DTO if necessary, otherwise return directly
		var dto = category.Value.Adapt<CategoryDto>();

		return Result.Ok(dto);

	}

	/// <summary>
	///   Retrieves all category data asynchronously.
	/// </summary>
	/// <returns>
	///   A <see cref="Result{T}" /> containing a list of <see cref="CategoryDto" />
	///   or null if no data is found.
	/// </returns>
	public async Task<Result<List<CategoryDto>>> GetAllAsync()
	{

		var output = await _cache.GetAsync<List<CategoryDto>>(_cacheName);

		if (output is not null)
		{
			return Result<List<CategoryDto>>.Ok(output);
		}

		var results = await _repository.GetAllAsync();

		// If the repository call fails, return the failure result
		if (results.Failure)
		{
			return Result<List<CategoryDto>>.Fail("Failed to retrieve categories.");
		}

		output = results.Value.Adapt<List<CategoryDto>>();

		await _cache.SetAsync(_cacheName, output, TimeSpan.FromDays(1));

		return Result<List<CategoryDto>>.Ok(output);

	}


	/// <summary>
	///   Updates an existing category asynchronously.
	/// </summary>
	/// <param name="category">The category data transfer object containing updated values for the category.</param>
	/// <returns>A <see cref="Result" /> indicating the success or failure of the update operation.</returns>
	public async Task<Result> UpdateAsync(CategoryDto? category)
	{

		if (category is null)
		{
			return Result.Fail("Category cannot be null");
		}

		await _cache.RemoveAsync(_cacheName);

		var result = await _repository.UpdateAsync(category.Id, category.Adapt<Category>());

		return result.Failure ? Result.Fail("Failed to update category") : Result.Ok();

	}

}