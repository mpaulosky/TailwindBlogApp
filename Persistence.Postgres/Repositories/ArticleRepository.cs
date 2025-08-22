// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =======================================================

using Domain.Models;

namespace Persistence.Postgres.Repositories;

/// <summary>
///   ArticleRepository class for Postgres
/// </summary>
public class ArticleRepository : Repository<Article>, IArticleRepository
{

	private readonly PgContext? _context;

	/// <summary>
	///   Initializes a new instance of the <see cref="ArticleRepository" /> class using Postgres.
	/// </summary>
	/// <param name="context">The Postgres database context.</param>
	public ArticleRepository(PgContext? context) : base(context)
	{

		_context = context ?? throw new ArgumentNullException(nameof(context));

	}

	/// <summary>
	///   Gets articles by user asynchronously using Postgres.
	/// </summary>
	/// <param name="entity">The user DTO.</param>
	/// <returns>A result containing the articles for the user.</returns>
	public async Task<Result<IEnumerable<Article>>> GetByUserAsync(AppUserDto entity)
	{
		if (string.IsNullOrWhiteSpace(entity.Id))
		{
			return Result.Fail<IEnumerable<Article>>("User ID cannot be empty");
		}

		if (_context == null)
		{
			return Result.Fail<IEnumerable<Article>>("Database context is not available");
		}

		var articles = await _context.Articles
				.Where(a => a.AuthorId == entity.Id)
				.ToListAsync();

		return articles.Count == 0
				? Result.Fail<IEnumerable<Article>>($"No articles found for user with ID {entity.Id}")
				: Result.Ok<IEnumerable<Article>>(articles);
	}

}