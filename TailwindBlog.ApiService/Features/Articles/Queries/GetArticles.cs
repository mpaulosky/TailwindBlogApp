// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     GetArticles.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using TailwindBlog.Domain.Extensions;

namespace TailwindBlog.ApiService.Features.Articles.Queries;

public record GetArticlesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PaginationModel<Article>>>;

public sealed class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, Result<PaginationModel<Article>>>
{
	private readonly IArticleRepository _articleRepository;

	public GetArticlesQueryHandler(IArticleRepository articleRepository)
	{
		_articleRepository = articleRepository;
	}

	public async Task<Result<PaginationModel<Article>>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var query = (await _articleRepository.GetAllAsync()).AsQueryable();
			var totalCount = query.Count();
			var articles = query
					.Paginate(request.PageNumber, request.PageSize)
					.ToList();

			var paginatedResult = PaginationModel<Article>.Create(
					articles,
					totalCount,
					request.PageNumber,
					request.PageSize);

			return Result<PaginationModel<Article>>.Ok(paginatedResult);
		}
		catch (Exception ex)
		{
			return Result<PaginationModel<Article>>.Fail($"Failed to retrieve articles: {ex.Message}");
		}
	}
}
