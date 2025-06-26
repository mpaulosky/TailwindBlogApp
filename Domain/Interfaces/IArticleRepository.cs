// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     IArticleRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Interfaces;

public interface IArticleRepository : IRepository<Article>
{

	Task<Result<IEnumerable<Article>>> GetByUserAsync(AppUserDto entity);

}