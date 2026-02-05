// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ICategoryService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =============================================

namespace Domain.Interfaces;

public interface ICategoryService
{

	Task<Result> ArchiveAsync(CategoryDto? category);

	Task<Result> CreateAsync(CategoryDto? category);

	Task<Result<CategoryDto>> GetAsync(Guid categoryId);

	Task<Result<List<CategoryDto>>> GetAllAsync();

	Task<Result> UpdateAsync(CategoryDto? category);

}