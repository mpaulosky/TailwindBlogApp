// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DeleteArticleCommandValidator.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using FluentValidation;
using MongoDB.Bson;
using TailwindBlog.ApiService.Features.Articles.Commands;

namespace TailwindBlog.ApiService.Features.Articles.Validators;

public sealed class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
{
	public DeleteArticleCommandValidator()
	{
		RuleFor(x => x.ArticleId)
			.NotEqual(Guid.Empty).WithMessage("ArticleId is required.");
	}
}
