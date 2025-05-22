// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

public sealed class ArticleRepository : Repository<Article>, IArticleRepository
{
	private const string _collectionName = "articles";

	public ArticleRepository(IMongoDatabase database)
		: base(database, _collectionName)
	{ }

	public async Task<List<Article>?> GetByUserAsync(string userId)
	{
		return await Collection.Find(a => a.Author.Id == userId).ToListAsync();
	}

}