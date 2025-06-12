// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     IArticleService.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =======================================================

namespace Persistence.Services;

public interface IArticleService
{

	Task Archive(ArticleDto article);

	Task Create(ArticleDto article);

	Task<Article> Get(ObjectId articleId);

	Task<List<Article>> GetAll();

	Task Update(ArticleDto article);

}