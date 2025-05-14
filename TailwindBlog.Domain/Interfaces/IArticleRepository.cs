// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     IArticleRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Interfaces;

public interface IArticleRepository
{

	void Add(Article entity);

	Task AddRangeAsync(IEnumerable<Article> entities);

	Task<bool> AnyAsync(Expression<Func<Article, bool>> predicate);

	Task<IEnumerable<Article>> FindAsync(Expression<Func<Article, bool>> predicate);

	Task<Article?> FindFirstAsync(Expression<Func<Article, bool>> predicate);

	Task<IEnumerable<Article>> GetAllAsync();

	Task<Article?> GetByIdAsync(Guid id);

	Task<List<Article>?> GetByUserAsync(string userId);

	void Remove(Article entity);

	void Update(Article entity);

}
