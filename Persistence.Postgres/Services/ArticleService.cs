// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleService.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =======================================================

using Domain.Models;

namespace Persistence.Postgres.Services;

/// <summary>
///   ArticleService class
/// </summary>
public class ArticleService : IArticleService
{

	private const string _cacheName = "ArticleData";

	private readonly ICacheService _cache;

	private readonly IArticleRepository _repository;

	/// <summary>
	///   ArticleService constructor
	/// </summary>
	/// <param name="repository">IArticleRepository</param>
	/// <param name="cache">ICacheService</param>
	/// <exception cref="ArgumentNullException"></exception>
	public ArticleService(IArticleRepository repository, ICacheService cache)
	{
		ArgumentNullException.ThrowIfNull(repository);
		ArgumentNullException.ThrowIfNull(cache);

		_repository = repository;
		_cache = cache;
	}

	/// <summary>
	///   Archives a given article.
	/// </summary>
	/// <param name="article">The article to be archived. Must not be null.</param>
	/// <returns>
	///   A <see cref="Result" /> indicating the success or failure of the operation. If the operation fails,
	///   the result contains an error message.
	/// </returns>
	public async Task<Result> ArchiveAsync(ArticleDto? article)
	{

		// Validate input
		if (article == null)
		{
			return Result.Fail("Article cannot be null.");
		}

		// Remove the cache for categories so it's refreshed after the change
		await _cache.RemoveAsync(_cacheName);

		try
		{

			await _repository.ArchiveAsync(article.Adapt<Article>());

			return Result.Ok();

		}
		catch (Exception ex)
		{

			return Result.Fail($"Failed to archive article: {ex.Message}");

		}

	}

	/// <summary>
	///   Asynchronously creates a new article.
	/// </summary>
	/// <param name="article">The article data transfer object to be created.</param>
	/// <returns>
	///   A <see cref="Result" /> indicating the success or failure of the operation. If the operation fails,
	///   the result contains an error message.
	/// </returns>
	public async Task<Result> CreateAsync(ArticleDto? article)
	{

		// Validate input
		if (article == null)
		{
			return Result.Fail("Article cannot be null.");
		}

		// Remove the cache so it's refreshed on next GetAll
		await _cache.RemoveAsync(_cacheName);

		try
		{

			// Call a repository to create
			await _repository.CreateAsync(article.Adapt<Article>());

			return Result.Ok();

		}
		catch (Exception ex)
		{

			// Handle or log exception as necessary
			return Result.Fail($"Failed to create article: {ex.Message}");

		}

	}

	/// <summary>
	///   Retrieves all articles created by a specific user.
	/// </summary>
	/// <param name="entity">The user whose articles are to be retrieved.</param>
	/// <returns>
	///   A <see cref="Result{T}" /> containing a list of articles created by the specified user, or an error message if
	///   the operation fails.
	/// </returns>
	public async Task<Result<List<ArticleDto>>> GetByUserAsync(AppUserDto? entity)
	{

		// Validate input user
		if (entity == null || string.IsNullOrEmpty(entity.Id))
		{
			return Result<List<ArticleDto>>.Fail("Invalid user.");
		}

		// Query repository for all articles
		var allArticlesResult = await GetAllAsync();

		if (allArticlesResult.Failure || allArticlesResult.Value is null)
		{
			return Result<List<ArticleDto>>.Fail("Could not retrieve articles.");
		}

		// Filter articles where the author matches the given 
		var allArticles = allArticlesResult.Value;

		var userArticles = allArticles
				.Where(a => a.Author.Id == entity.Id)
				.ToList();

		return userArticles.Count > 0
				? Result<List<ArticleDto>>.Ok(userArticles)
				: Result<List<ArticleDto>>.Fail("No articles found for the specified user.");

	}

	/// <summary>
	///   Retrieves all article data asynchronously.
	/// </summary>
	/// <returns>
	///   A <see cref="Result{T}" /> containing a list of <see cref="ArticleDto" />
	///   or null if no data is found.
	/// </returns>
	public async Task<Result<List<ArticleDto>?>> GetAllAsync()
	{

		var output = await _cache.GetAsync<List<ArticleDto>>(_cacheName);

		if (output is not null)
		{
			return Result<List<ArticleDto>?>.Ok(output);
		}

		var results = await _repository.GetAllAsync();

		// If the repository call fails, return the failure result
		if (results.Failure)
		{
			return Result<List<ArticleDto>?>.Fail("Failed to retrieve articles.");
		}

		output = results.Value.Adapt<List<ArticleDto>>().ToList();

		await _cache.SetAsync(_cacheName, output, TimeSpan.FromDays(1));

		return Result<List<ArticleDto>?>.Ok(output);

	}


	/// <summary>
	///   Updates an existing article asynchronously.
	/// </summary>
	/// <param name="article">The article data transfer object containing updated values for the article.</param>
	/// <returns>A <see cref="Result" /> indicating the success or failure of the update operation.</returns>
	public async Task<Result> UpdateAsync(ArticleDto? article)
	{

		if (article is null)
		{
			return Result.Fail("Article cannot be null");
		}

		await _cache.RemoveAsync(_cacheName);

		var result = await _repository.UpdateAsync(article.Id, article.Adapt<Article>());

		return result.Failure ? Result.Fail("Failed to update article") : Result.Ok();

	}

	/// <summary>
	///   Retrieves a article by its unique identifier.
	/// </summary>
	/// <param name="articleId">The unique identifier of the article to retrieve.</param>
	/// <returns>
	///   A <see cref="Result{T}" /> containing a <see cref="ArticleDto" /> if successful, or a failure result with an
	///   error message.
	/// </returns>
	public async Task<Result<ArticleDto>> GetAsync(Guid articleId)
	{

		// Validate input
		if (articleId == Guid.Empty)
		{
			return Result<ArticleDto>.Fail("Article id cannot be empty.");
		}

		// Try to get all categories from the cache
		var articleList = await _cache.GetAsync<List<ArticleDto>>(_cacheName);

		// If found in the cache, try to return the matching article
		var cachedArticle = articleList?.FirstOrDefault(c => c.Id == articleId);

		if (cachedArticle != null)
		{
			return Result<ArticleDto>.Ok(cachedArticle);
		}

		// Not cached, get from repository
		var article = await _repository.GetAsync(articleId);

		if (article.Failure)
		{
			return Result<ArticleDto>.Fail("Article not found.");
		}

		// Adapt model to DTO if necessary, otherwise return directly
		var dto = article.Value.Adapt<ArticleDto>();

		return Result.Ok(dto);

	}

}