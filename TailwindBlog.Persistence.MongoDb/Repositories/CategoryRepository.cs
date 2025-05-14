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

public sealed class CategoryRepository : Repository<Category>, ICategoryRepository
{
	public CategoryRepository(AppDbContext context)
		: base(context)
	{ }

	public async Task<IEnumerable<Category>> GetAllAsync()
	{
		return await DbSet.ToListAsync();
	}
}
