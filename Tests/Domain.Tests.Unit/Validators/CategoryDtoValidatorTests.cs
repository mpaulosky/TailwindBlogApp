// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryDtoValidatorTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Validators;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(CategoryDtoValidator))]
public class CategoryDtoValidatorTests
{

	[Fact]
	public void Should_Fail_When_Name_Is_Empty()
	{

		// Arrange
		var validator = new CategoryDtoValidator();
		var dto = new CategoryDto { Name = string.Empty, Description = "desc" };

		// Act
		var result = validator.Validate(dto);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(e => e.PropertyName == "Name");

	}

	[Fact]
	public void Should_Fail_When_Description_Is_Empty()
	{

		// Arrange
		var validator = new CategoryDtoValidator();
		var dto = new CategoryDto { Name = "name", Description = string.Empty };

		// Act
		var result = validator.Validate(dto);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(e => e.PropertyName == "Description");

	}

	[Fact]
	public void Should_Pass_With_Valid_Fields()
	{

		// Arrange
		var validator = new CategoryDtoValidator();
		var dto = new CategoryDto { Name = "ValidName", Description = "ValidDescription" };

		// Act
		var result = validator.Validate(dto);

		// Assert
		result.IsValid.Should().BeTrue();

	}


}