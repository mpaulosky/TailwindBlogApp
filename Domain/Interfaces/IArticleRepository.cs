// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     IArticleRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

using Domain.Abstractions;
using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces;

public interface IArticleRepository : IRepository<Article>
{

	Task<Result<IEnumerable<Article>>> GetByUserAsync(AppUserDto entity);

}