// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

internal sealed class CategoryRepository : Repository<Category>, ICategoryRepository
{

	public CategoryRepository(AppDbContext context)
	: base(context)
	{ }

}
