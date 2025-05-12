// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DeleteArticle.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using TailwindBlog.Domain.Abstractions;

namespace TailwindBlog.ApiService.Features.Articles.Commands;

public record DeleteArticleCommand(ObjectId ArticleId) : IRequest<Result<bool>>;

public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, Result<bool>>
{
	private readonly IArticleRepository _articleRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
	{
		_articleRepository = articleRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<bool>> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var article = await _articleRepository.GetByIdAsync(request.ArticleId);
			if (article is null)
			{
				return Result.Fail<bool>($"Article with ID {request.ArticleId} not found");
			}

			_articleRepository.Remove(article);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result.Ok(true);
		}
		catch (Exception ex)
		{
			return Result.Fail<bool>($"Failed to delete article: {ex.Message}");
		}
	}
}
