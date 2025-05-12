// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     GetArticleById.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using TailwindBlog.Domain.Abstractions;
using TailwindBlog.Domain.Entities;

namespace TailwindBlog.ApiService.Features.Articles.Queries;

public record GetArticleByIdQuery(ObjectId ArticleId) : IRequest<Result<Article>>;

public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, Result<Article>>
{
	private readonly IArticleRepository _articleRepository;

	public GetArticleByIdQueryHandler(IArticleRepository articleRepository)
	{
		_articleRepository = articleRepository;
	}

	public async Task<Result<Article>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var article = await _articleRepository.GetByIdAsync(request.ArticleId); return article is not null
					? Result<Article>.Ok(article)
					: Result<Article>.Fail($"Article with ID {request.ArticleId} not found");
		}
		catch (Exception ex)
		{
			return Result<Article>.Fail($"Failed to retrieve article: {ex.Message}");
		}
	}
}
