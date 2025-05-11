// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

internal sealed class ArticleRepository : Repository<Article>, IArticleRepository
{

	public ArticleRepository(AppDbContext context)
	: base(context)
	{ }

	public async Task<List<Article>?> GetByUserAsync(string userId)
	{

		return await Context.Articles
				.Where(a => a.Author.Id == userId)
				.ToListAsync();

	}

}
