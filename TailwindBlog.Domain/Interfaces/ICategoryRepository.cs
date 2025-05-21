// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ICategoryRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.Domain.Interfaces;

public interface ICategoryRepository
{

	void Add(Category entity);

	Task AddRangeAsync(IEnumerable<Category> entities);

	Task<bool> AnyAsync(Expression<Func<Category, bool>> predicate);

	Task<IEnumerable<Category>> FindAsync(Expression<Func<Category, bool>> predicate);

	Task<Category?> FindFirstAsync(Expression<Func<Category, bool>> predicate);

	Task<IEnumerable<Category>> GetAllAsync();

	Task<Category?> GetByIdAsync(ObjectId id);

	void Remove(Category entity);

	void Update(Category entity);

}