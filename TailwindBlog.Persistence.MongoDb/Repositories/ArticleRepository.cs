// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TailwindBlog.Domain.Entities;
using TailwindBlog.Domain.Interfaces;

namespace TailwindBlog.Persistence.Repositories;

public sealed class ArticleRepository : Repository<Article>, IArticleRepository
{

	public ArticleRepository(AppDbContext context)
	: base(context)
	{ }

	public async Task<List<Article>?> GetByUserAsync(string userId)
	{

		return await Context.Set<Article>()
				.Where(a => a.Author.Id == userId)
				.ToListAsync();

	}

	public async Task<IEnumerable<Article>> GetAllAsync()
	{
		return await DbSet.ToListAsync();
	}
}
