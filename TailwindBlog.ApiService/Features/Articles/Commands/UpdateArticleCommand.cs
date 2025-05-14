// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     UpdateArticle.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using MongoDB.Bson;
using TailwindBlog.Domain.Entities;
using TailwindBlog.Domain.Abstractions;

namespace TailwindBlog.ApiService.Features.Articles.Commands;

/// <summary>
/// Command to update an article by its ObjectId.
/// </summary>
public record UpdateArticleCommand(Guid ArticleId, Article Article) : IRequest<Result<Article>>;

public sealed class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, Result<Article>>
{
	private readonly IArticleRepository _articleRepository;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
	{
		_articleRepository = articleRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Article>> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
	{
		try
		{
			if (request is null)
			{
				return Result<Article>.Fail("Request is null.");
			}

			var article = await _articleRepository.GetByIdAsync(request.ArticleId);
			if (article is null)
			{
				return Result<Article>.Fail($"Article with ID '{request.ArticleId}' not found.");
			}

			article.Update(
				request.Article.Title,
				request.Article.Introduction,
				request.Article.CoverImageUrl,
				request.Article.UrlSlug,
				request.Article.Author,
				request.Article.IsPublished,
				request.Article.PublishedOn
			);
			article.ModifiedOn = DateTime.UtcNow;

			_articleRepository.Update(article);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<Article>.Ok(article);
		}
		catch (Exception ex)
		{
			return Result<Article>.Fail($"Failed to update article: {ex.Message}");
		}
	}
}
