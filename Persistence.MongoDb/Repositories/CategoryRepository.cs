// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =======================================================

namespace Persistence.Repositories;

/// <summary>
///   CategoryRepository class
/// </summary>
public class CategoryRepository : Repository<Category>, ICategoryRepository
{

	public CategoryRepository(IMongoDbContextFactory context) : base(context) { }

}