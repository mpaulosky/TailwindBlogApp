// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     IApplicationDbContext.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Interfaces;

public interface IApplicationDbContext
{

	IMongoCollection<Article> Articles { get; }

	IMongoCollection<Category> Categories { get; }

}