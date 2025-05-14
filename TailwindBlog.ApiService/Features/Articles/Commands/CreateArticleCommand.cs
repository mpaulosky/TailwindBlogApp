// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CreateArticle.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.ApiService.Features.Articles.Commands;

public record CreateArticleCommand(Article Article) : IRequest<Result<Article>>;

public sealed class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, Result<Article>>
{
	private readonly IArticleRepository _articleRepository;
	private readonly IUnitOfWork _unitOfWork;

	public CreateArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
	{
		_articleRepository = articleRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Article>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
	{
		try
		{
			_articleRepository.Add(request.Article);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result<Article>.Ok(request.Article);
		}
		catch (Exception ex)
		{
			return Result<Article>.Fail($"Failed to create article: {ex.Message}");
		}
	}
}
