// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

/// <summary>
///   ArticleRepository class
/// </summary>
public class ArticleRepository : Repository<Article>, IArticleRepository
{

	public ArticleRepository(IMongoDbContextFactory context) : base(context) { }

	public async Task<Result<IEnumerable<Article>>> GetByUserAsync(AppUserDto entity)
	{

		try
		{

			var entityId = entity.Id;

			if (entityId == string.Empty)
			{
				return Result.Fail<IEnumerable<Article>>("Article ID cannot be empty");
			}

			// Use the Author's ID to filter articles
			var filter = Builders<Article>.Filter.Eq(a => a.Author.Id, entityId);

			var result = await Collection.FindAsync(filter);

			var articles = await result.ToListAsync();

			return articles.Count == 0
					? Result.Fail<IEnumerable<Article>>($"No articles found for user with ID {entityId}")
					: Result.Ok<IEnumerable<Article>>(articles);

		}
		catch (Exception ex)
		{

			return Result.Fail<IEnumerable<Article>>(ex.Message);

		}

	}

}