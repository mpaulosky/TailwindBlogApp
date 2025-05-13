// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleDto.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.ApiService.Features.Articles.Models;

public record ArticleDto(string Title, string Introduction, string CoverImageUrl, string UrlSlug, string AuthorId, bool IsPublished, DateTime? PublishedOn);
