// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     CategoryValidator.cs
// Company:       mpaulosky
// Author:        Matthew
// Solution Name: TailwindBlog
// Project Name:  TailwindBlog.Domain
// =======================================================

using FluentValidation;
using TailwindBlog.Domain.Entities;

namespace TailwindBlog.Domain.Validators;

/// <summary>
/// Validator for the <see cref="Category"/> entity.
/// </summary>
public class CategoryValidator : AbstractValidator<Category>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CategoryValidator"/> class.
	/// </summary>
	public CategoryValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Name is required")
			.MaximumLength(80).WithMessage("Name cannot be longer than 80 characters");

		RuleFor(x => x.Description)
			.NotEmpty().WithMessage("Description is required")
			.MaximumLength(100).WithMessage("Description cannot be longer than 100 characters");

		RuleFor(x => x.UrlSlug)
			.MaximumLength(100).WithMessage("URL slug cannot be longer than 100 characters")
			.Matches("^[a-z0-9-]*$").When(x => !string.IsNullOrEmpty(x.UrlSlug))
			.WithMessage("URL slug can only contain lowercase letters, numbers, and hyphens");
	}
}
