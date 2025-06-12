// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ICategoryService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =============================================

namespace Persistence.Services;

public interface ICategoryService
{

	Task Archive(CategoryDto category);

	Task Create(CategoryDto category);

	Task<Category> Get(ObjectId categoryId);

	Task<List<Category>> GetAll();

	Task Update(CategoryDto category);

}