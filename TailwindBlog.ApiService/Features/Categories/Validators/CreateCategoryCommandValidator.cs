// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CreateCategoryCommandValidator.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using FluentValidation;

namespace TailwindBlog.ApiService.Features.Categories.Validators;

public class CreateCategoryCommandValidator : AbstractValidator<Commands.CreateCategoryCommand>
{
	public CreateCategoryCommandValidator()
	{
		RuleFor(x => x.Category.Name)
			.NotEmpty().WithMessage("Name is required.")
			.MaximumLength(50);
	}
}
