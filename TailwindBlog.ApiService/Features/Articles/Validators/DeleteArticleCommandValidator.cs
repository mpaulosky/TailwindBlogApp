// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DeleteArticleCommandValidator.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using FluentValidation;

namespace TailwindBlog.ApiService.Features.Articles.Validators;

public class DeleteArticleCommandValidator : AbstractValidator<Commands.DeleteArticleCommand>
{
	public DeleteArticleCommandValidator()
	{
		RuleFor(x => x.ArticleId)
			.NotEmpty().WithMessage("ArticleId is required.");
	}
}
