// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryValidatorTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Validators;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(CategoryValidator))]
public class CategoryValidatorTests
{

	private readonly CategoryValidator _validator;

	public CategoryValidatorTests()
	{

		_validator = new CategoryValidator();

	}

	[Fact]
	public void Validate_WhenAllPropertiesValid_ShouldNotHaveErrors()
	{

		// Arrange
		var category = FakeCategory.GetNewCategory(true);

		// Act
		var result = _validator.Validate(category);

		// Assert
		result.IsValid.Should().BeTrue();
		result.Errors.Should().BeEmpty();

	}

	[Theory]
	[InlineData("", "slug", "Name is required")]
	[InlineData("name", "", "Slug is required")]
	public void Validate_WhenRequiredPropertyMissing_ShouldHaveError(
			string name,
			string slug,
			string expectedError)
	{

		// Arrange
		var category = new Category(
				name,
				slug,
				true
		);

		// Act
		var result = _validator.Validate(category);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.ErrorMessage == expectedError);

	}

	[Theory]
	[InlineData("name", "slug", 81, "Name")]
	[InlineData("name", "slug", 101, "Slug")]
	public void Validate_WhenPropertyTooLong_ShouldHaveError(
			string name,
			string slug,
			int length,
			string propertyName)
	{

		// Arrange
		var category = new Category(
				propertyName == "Name" ? new string('x', length) : name,
				propertyName == "Slug" ? new string('x', length) : slug,
				true
		);

		// Act
		var result = _validator.Validate(category);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == propertyName);

	}

}