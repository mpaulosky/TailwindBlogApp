// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryValidator.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Validation;

using FluentValidation;
using Entities;

public class CategoryValidator : AbstractValidator<Category>
{
	public CategoryValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Name is required")
			.MaximumLength(80);
		// Removed Slug validation
	}
}