// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryDtoTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Models;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(CategoryDto))]
public class CategoryDtoTests
{

	[Fact]
	public void CategoryDto_WhenCreated_ShouldHaveEmptyProperties()
	{

		// Arrange & Act
		var category = new CategoryDto();

		// Assert
		category.Id.Should().Be(Guid.Empty);
		category.Name.Should().BeEmpty();
		category.CreatedOn.Should().Be(DateTime.MinValue);
		category.ModifiedOn.Should().BeNull();

	}

	[Fact]
	public void CategoryDto_Empty_ShouldReturnEmptyInstance()
	{

		// Arrange & Act
		var category = CategoryDto.Empty;

		// Assert
		category.Id.Should().Be(Guid.Empty);
		category.Name.Should().BeEmpty();
		category.CreatedOn.Should().Be(DateTime.MinValue);
		category.ModifiedOn.Should().BeNull();

	}

	[Theory]
	[InlineData("Test Name")]
	[InlineData("Another Name")]
	public void CategoryDto_WhenPropertiesSet_ShouldHaveCorrectValues(
			string name)
	{

		// Arrange & Act
		var now = DateTime.UtcNow;

		var category = FakeCategoryDto.GetNewCategoryDto(true);
		category.Name = name;

		// Assert
		category.Name.Should().Be(name);
		category.CreatedOn.Should().Be(now);
		category.ModifiedOn.Should().BeNull();

	}

	[Theory]
	[InlineData("", "Name is required")]
	public void CategoryDto_WhenCreated_ShouldValidateRequiredFields(
			string name,
			string expectedError)
	{

		// Arrange & Act & Assert
		FluentActions.Invoking(() => new CategoryDto(
						Guid.CreateVersion7(),
						name,
						DateTime.UtcNow,
						null
				)).Should().Throw<ValidationException>()
				.WithMessage($"*{expectedError}*");

	}

}