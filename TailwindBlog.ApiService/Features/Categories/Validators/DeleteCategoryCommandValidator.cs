// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DeleteCategoryCommandValidator.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using FluentValidation;

namespace TailwindBlog.ApiService.Features.Categories.Validators;

public class DeleteCategoryCommandValidator : AbstractValidator<Commands.DeleteCategoryCommand>
{
	public DeleteCategoryCommandValidator()
	{
		RuleFor(x => x.CategoryId)
			.NotEmpty().WithMessage("CategoryId is required.");
	}
}
