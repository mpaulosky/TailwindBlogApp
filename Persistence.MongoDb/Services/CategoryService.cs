// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CategoryService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =============================================

// namespace TailwindBlog.Persistence.Services;
//
// /// <summary>
// ///   CategoryService class
// /// </summary>
// public class CategoryService : ICategoryService
// {
// 	private const string _cacheName = "CategoryData";
// 	private readonly IMemoryCache _cache;
// 	private readonly ICategoryRepository _repository;
//
// 	/// <summary>
// 	///   CategoryService constructor
// 	/// </summary>
// 	/// <param name="repository">ICategoryRepository</param>
// 	/// <param name="cache">IMemoryCache</param>
// 	/// <exception cref="ArgumentNullException"></exception>
// 	public CategoryService(ICategoryRepository repository, IMemoryCache cache)
// 	{
// 		ArgumentNullException.ThrowIfNull(repository);
// 		ArgumentNullException.ThrowIfNull(cache);
//
// 		_repository = repository;
// 		_cache = cache;
// 	}
//
// 	/// <summary>
// 	///   CreateCategory method
// 	/// </summary>
// 	/// <param name="category">CategoryDto</param>
// 	/// <returns>Task</returns>
// 	/// <exception cref="ArgumentNullException"></exception>
// 	public Task CreateCategory(CategoryDto category)
// 	{
// 		ArgumentNullException.ThrowIfNull(category);
//
// 		_cache.Remove(_cacheName);
//
// 		return _repository.CreateAsync(category);
// 	}
//
// 	/// <summary>
// 	///   ArchiveCategory method
// 	/// </summary>
// 	/// <param name="category">CategoryDto</param>
// 	/// <returns>Task</returns>
// 	/// <exception cref="ArgumentNullException"></exception>
// 	public Task ArchiveCategory(CategoryDto category)
// 	{
// 		ArgumentNullException.ThrowIfNull(category);
//
// 		_cache.Remove(_cacheName);
//
// 		return _repository.ArchiveAsync(category);
// 	}
//
// 	/// <summary>
// 	///   GetCategory method
// 	/// </summary>
// 	/// <param name="categoryId">string</param>
// 	/// <returns>Task of CategoryDto</returns>
// 	/// <exception cref="ArgumentException">ThrowIfNullOrEmpty(categoryId)</exception>
// 	public async Task<CategoryDto> GetCategory(string? categoryId)
// 	{
// 		ArgumentException.ThrowIfNullOrEmpty(categoryId);
//
// 		CategoryDto result = await _repository.GetAsync(categoryId);
//
// 		return result;
// 	}
//
// 	/// <summary>
// 	///   GetCategories method
// 	/// </summary>
// 	/// <returns>Task of List CategoryDto</returns>
// 	public async Task<List<CategoryDto>> GetCategories()
// 	{
// 		List<CategoryDto>? output = _cache.Get<List<CategoryDto>>(_cacheName);
//
// 		if (output is not null)
// 		{
// 			return output;
// 		}
//
// 		IEnumerable<CategoryDto> results = await _repository.GetAllAsync();
//
// 		output = results.ToList();
//
// 		_cache.Set(_cacheName, output, TimeSpan.FromDays(1));
//
// 		return output;
// 	}
//
// 	/// <summary>
// 	///   UpdateCategory method
// 	/// </summary>
// 	/// <param name="category">CategoryDto</param>
// 	/// <returns>Task</returns>
// 	/// <exception cref="ArgumentNullException"></exception>
// 	public Task UpdateCategory(CategoryDto category)
// 	{
// 		ArgumentNullException.ThrowIfNull(category);
//
// 		_cache.Remove(_cacheName);
//
// 		return _repository.UpdateAsync(category.Id, category);
// 	}
// }