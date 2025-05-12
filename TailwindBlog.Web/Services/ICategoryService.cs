// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     ICategoryService.cs
// Project Name:  TailwindBlog.Web
// =======================================================

namespace TailwindBlog.Web.Services;

public interface ICategoryService
{
	Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
	Task<Category?> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
	Task<Result<Category>> CreateCategoryAsync(Category category, CancellationToken cancellationToken = default);
	Task<Result<Category>> UpdateCategoryAsync(string id, Category category, CancellationToken cancellationToken = default);
	Task<Result> DeleteCategoryAsync(string id, CancellationToken cancellationToken = default);
}
