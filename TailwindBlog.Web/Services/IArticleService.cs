// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     IArticleService.cs
// Project Name:  TailwindBlog.Web
// =======================================================

namespace TailwindBlog.Web.Services;

public interface IArticleService
{
	Task<IEnumerable<Article>> GetArticlesAsync(CancellationToken cancellationToken = default);
	Task<Article?> GetArticleByIdAsync(string id, CancellationToken cancellationToken = default);
	Task<Result<Article>> CreateArticleAsync(Article article, CancellationToken cancellationToken = default);
	Task<Result<Article>> UpdateArticleAsync(string id, Article article, CancellationToken cancellationToken = default);
	Task<Result> DeleteArticleAsync(string id, CancellationToken cancellationToken = default);
}
