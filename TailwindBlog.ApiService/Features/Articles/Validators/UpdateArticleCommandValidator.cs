// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     UpdateArticleCommandValidator.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using FluentValidation;
using MongoDB.Bson;
using TailwindBlog.ApiService.Features.Articles.Commands;

namespace TailwindBlog.ApiService.Features.Articles.Validators;

public sealed class UpdateArticleCommandValidator : AbstractValidator<UpdateArticleCommand>
{
	public UpdateArticleCommandValidator()
	{
		RuleFor(x => x.ArticleId)
			.NotEqual(Guid.Empty).WithMessage("ArticleId is required.");
		RuleFor(x => x.Article.Title)
			.NotEmpty().WithMessage("Title is required.")
			.MaximumLength(120);
		RuleFor(x => x.Article.Introduction)
			.NotEmpty().WithMessage("Introduction is required.");
		RuleFor(x => x.Article.CoverImageUrl)
			.NotEmpty().WithMessage("Cover image is required.");
		RuleFor(x => x.Article.Author)
			.NotNull().WithMessage("Author is required.");
	}
}
