// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     CategoryValidatorTests.cs
// Company:       mpaulosky
// Author:        Matthew
// Solution Name: TailwindBlog
// Project Name:  TailwindBlog.Domain.Tests.Unit
// =======================================================

using FluentValidation.TestHelper;
using TailwindBlog.Domain.Validators;

namespace TailwindBlog.Domain.Tests.Unit.Validators;

[ExcludeFromCodeCoverage]
public class CategoryValidatorTests
{
	private readonly CategoryValidator _validator = new();
	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	public void Name_WhenEmpty_ShouldHaveError(string? name)
	{
		// Arrange
		var category = new Category("valid name", "valid description");
		typeof(Category).GetProperty(nameof(Category.Name))!
			.SetValue(category, name, null);

		// Act & Assert
		var result = _validator.TestValidate(category);
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("Name is required");
	}

	[Fact]
	public void Name_WhenTooLong_ShouldHaveError()
	{
		// Arrange
		var category = new Category(new string('a', 81), "valid description");

		// Act & Assert
		var result = _validator.TestValidate(category);
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("Name cannot be longer than 80 characters");
	}
	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	public void Description_WhenEmpty_ShouldHaveError(string? description)
	{
		// Arrange
		var category = new Category("valid name", "valid description");
		typeof(Category).GetProperty(nameof(Category.Description))!
			.SetValue(category, description, null);

		// Act & Assert
		var result = _validator.TestValidate(category);
		result.ShouldHaveValidationErrorFor(x => x.Description)
			.WithErrorMessage("Description is required");
	}

	[Fact]
	public void Description_WhenTooLong_ShouldHaveError()
	{
		// Arrange
		var category = new Category("valid name", new string('a', 101));

		// Act & Assert
		var result = _validator.TestValidate(category);
		result.ShouldHaveValidationErrorFor(x => x.Description)
			.WithErrorMessage("Description cannot be longer than 100 characters");
	}

	[Theory]
	[InlineData("invalid-SLUG")]
	[InlineData("invalid slug")]
	[InlineData("invalid_slug")]
	[InlineData("@invalid-slug")]
	public void UrlSlug_WhenInvalidFormat_ShouldHaveError(string urlSlug)
	{
		// Arrange
		var category = new Category("valid name", "valid description");
		typeof(Category).GetProperty(nameof(Category.UrlSlug))!
			.SetValue(category, urlSlug, null);

		// Act & Assert
		var result = _validator.TestValidate(category);
		result.ShouldHaveValidationErrorFor(x => x.UrlSlug)
			.WithErrorMessage("URL slug can only contain lowercase letters, numbers, and hyphens");
	}

	[Fact]
	public void UrlSlug_WhenTooLong_ShouldHaveError()
	{
		// Arrange
		var category = new Category("valid name", "valid description");
		typeof(Category).GetProperty(nameof(Category.UrlSlug))!
			.SetValue(category, new string('a', 101), null);

		// Act & Assert
		var result = _validator.TestValidate(category);
		result.ShouldHaveValidationErrorFor(x => x.UrlSlug)
			.WithErrorMessage("URL slug cannot be longer than 100 characters");
	}

	[Theory]
	[InlineData("valid-slug")]
	[InlineData("another-valid-slug")]
	[InlineData("123-valid-slug")]
	public void UrlSlug_WhenValidFormat_ShouldNotHaveError(string urlSlug)
	{
		// Arrange
		var category = new Category("valid name", "valid description");
		typeof(Category).GetProperty(nameof(Category.UrlSlug))!
			.SetValue(category, urlSlug, null);

		// Act & Assert
		var result = _validator.TestValidate(category);
		result.ShouldNotHaveValidationErrorFor(x => x.UrlSlug);
	}
}
