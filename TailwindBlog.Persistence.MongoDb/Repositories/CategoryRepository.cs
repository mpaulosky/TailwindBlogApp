// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

public sealed class CategoryRepository : Repository<Category>, ICategoryRepository
{

	public CategoryRepository(AppDbContext context)
	: base(context)
	{ }

}
