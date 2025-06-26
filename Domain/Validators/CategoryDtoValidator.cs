// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryDtoValidator.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Validators;

/// <summary>
///   Validator for the <see cref="Category" /> entity.
/// </summary>
public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{

	/// <summary>
	///   Initializes a new instance of the <see cref="CategoryValidator" /> class.
	/// </summary>
	public CategoryDtoValidator()
	{

		RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Name is required")
				.MaximumLength(80);

		RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Description is required")
				.MaximumLength(100);

	}

}