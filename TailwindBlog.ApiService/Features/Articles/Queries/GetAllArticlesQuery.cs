// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     GetAllArticlesQuery.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.ApiService.Features.Articles.Queries;

public record GetAllArticlesQuery() : IRequest<Result<IEnumerable<Article>>>;

public sealed class GetAllArticlesQueryHandler : IRequestHandler<GetAllArticlesQuery, Result<IEnumerable<Article>>>
{
	private readonly IArticleRepository _articleRepository;

	public GetAllArticlesQueryHandler(IArticleRepository articleRepository)
	{
		_articleRepository = articleRepository;
	}

	public async Task<Result<IEnumerable<Article>>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var articles = await _articleRepository.GetAllAsync();
			return Result<IEnumerable<Article>>.Ok(articles);
		}
		catch (Exception ex)
		{
			return Result<IEnumerable<Article>>.Fail($"Failed to get articles: {ex.Message}");
		}
	}
}
