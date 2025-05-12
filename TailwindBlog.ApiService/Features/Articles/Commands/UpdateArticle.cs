// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     UpdateArticle.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using TailwindBlog.Domain.Abstractions;
using TailwindBlog.Domain.Entities;

namespace TailwindBlog.ApiService.Features.Articles.Commands;

public record UpdateArticleCommand(Article Article) : IRequest<Result<Article>>;

public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, Result<Article>>
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
			if (await _articleRepository.GetByIdAsync(request.Article.Id) is null)
			{
				return Result<Article>.Fail($"Article with ID {request.Article.Id} not found");
			}

			_articleRepository.Update(request.Article);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result<Article>.Ok(request.Article);
		}
		catch (Exception ex)
		{
			return Result<Article>.Fail($"Failed to update article: {ex.Message}");
		}
	}
}
