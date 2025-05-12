// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     EndpointExtensions.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using TailwindBlog.ApiService.Features.Articles;
using TailwindBlog.ApiService.Features.Categories;

namespace TailwindBlog.ApiService.Extensions;

public static class EndpointExtensions
{
	public static WebApplication MapArticleEndpoints(this WebApplication app)
	{
		ArticleEndpoints.MapArticleEndpoints(app);
		return app;
	}

	public static WebApplication MapCategoryEndpoints(this WebApplication app)
	{
		CategoryEndpoints.MapCategoryEndpoints(app);
		return app;
	}
}
