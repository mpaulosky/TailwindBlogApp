// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CreateArticleCommandValidator.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using FluentValidation;

namespace TailwindBlog.ApiService.Features.Articles.Validators;

public class CreateArticleCommandValidator : AbstractValidator<Commands.CreateArticleCommand>
{
	public CreateArticleCommandValidator()
	{
		RuleFor(x => x.Article.Title)
			.NotEmpty().WithMessage("Title is required.")
			.MaximumLength(100);
		RuleFor(x => x.Article.Introduction)
			.NotEmpty().WithMessage("Introduction is required.");
		RuleFor(x => x.Article.CoverImageUrl)
			.NotEmpty().WithMessage("Cover image is required.");
		RuleFor(x => x.Article.Author)
			.NotNull().WithMessage("Author is required.");
	}
}
