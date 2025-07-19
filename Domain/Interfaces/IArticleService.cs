// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     IArticleService.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =======================================================

namespace Domain.Interfaces;

public interface IArticleService
{

	Task<Result> ArchiveAsync(ArticleDto? article);

	Task<Result> CreateAsync(ArticleDto? article);

	Task<Result<ArticleDto>> GetAsync(Guid articleId);

	Task<Result<List<ArticleDto>>> GetByUserAsync(AppUserDto? entity);

	Task<Result<List<ArticleDto>>> GetAllAsync();

	Task<Result> UpdateAsync(ArticleDto? article);

}