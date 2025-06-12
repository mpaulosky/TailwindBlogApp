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
		category.Id.Should().Be(ObjectId.Empty);
		category.Name.Should().BeEmpty();
		category.Description.Should().BeEmpty();
		category.CreatedOn.Should().Be(DateTime.MinValue);
		category.ModifiedOn.Should().BeNull();

	}

	[Fact]
	public void CategoryDto_Empty_ShouldReturnEmptyInstance()
	{

		// Arrange & Act
		var category = CategoryDto.Empty;

		// Assert
		category.Id.Should().Be(ObjectId.Empty);
		category.Name.Should().BeEmpty();
		category.Description.Should().BeEmpty();
		category.CreatedOn.Should().Be(DateTime.MinValue);
		category.ModifiedOn.Should().BeNull();

	}

	[Theory]
	[InlineData("Test Name", "Test Description")]
	[InlineData("Another Name", "Another Description")]
	public void CategoryDto_WhenPropertiesSet_ShouldHaveCorrectValues(
			string name,
			string description)
	{

		// Arrange & Act
		var now = DateTime.UtcNow;

		var category = new CategoryDto(
				ObjectId.GenerateNewId(),
				name,
				description,
				now,
				null
		);

		// Assert
		category.Name.Should().Be(name);
		category.Description.Should().Be(description);
		category.CreatedOn.Should().Be(now);
		category.ModifiedOn.Should().BeNull();

	}

	[Theory]
	[InlineData("", "description", "Name is required")]
	[InlineData("Name", "", "Description is required")]
	public void CategoryDto_WhenCreated_ShouldValidateRequiredFields(
			string name,
			string description,
			string expectedError)
	{

		// Arrange & Act & Assert
		FluentActions.Invoking(() => new CategoryDto(
						ObjectId.GenerateNewId(),
						name,
						description,
						DateTime.UtcNow,
						null
				)).Should().Throw<ValidationException>()
				.WithMessage($"*{expectedError}*");

	}

}