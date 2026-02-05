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
	[InlineData("", "Name is required.")]
	public void Validate_WhenRequiredPropertyMissing_ShouldHaveError(
			string name,
			string expectedError)
	{

		// Arrange
		var category = new Category(
				name,
				true
		);

		// Act
		var result = _validator.Validate(category);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.ErrorMessage == expectedError);

	}

	[Theory]
	[InlineData(81, "Name")]
	public void Validate_WhenPropertyTooLong_ShouldHaveError(
			int length,
			string propertyName)
	{

		// Arrange
		var category = new Category(
				propertyName == "Name" ? new string('x', length) : "name",
				true
		);

		// Act
		var result = _validator.Validate(category);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == propertyName);

	}

}