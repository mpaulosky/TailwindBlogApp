// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Category.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Entities;

public class CategoryTests
{
	[Fact]
	public void Category_WhenCreated_ShouldHaveEmptyProperties()
	{
		// Arrange & Act
		var article = new Category(
			string.Empty,
			string.Empty,
			skipValidation: true
		);

		// Assert
		article.Name.Should().BeEmpty();
		article.Description.Should().BeEmpty();
		article.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
		article.ModifiedOn.Should().BeNull();
	}

	[Fact]
	public void Category_Empty_ShouldReturnEmptyInstance()
	{
		// Arrange & Act
		var article = Category.Empty;

		// Assert
		article.Id.Should().Be(ObjectId.Empty);
		article.Name.Should().BeEmpty();
		article.Description.Should().BeEmpty();
	}

	[Theory]
	[InlineData("Test Name", "Test Description")]
	[InlineData("Another Name", "Another Description")]
	public void Category_WhenPropertiesSet_ShouldHaveCorrectValues(
		string name,
		string description)
	{
		// Arrange & Act
		var article = new Category(
			name,
			description
		);

		// Assert
		article.Name.Should().Be(name);
		article.Description.Should().Be(description);
		article.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
		article.ModifiedOn.Should().BeNull(); // Default value
	}

	[Fact]
	public void Category_Update_ShouldUpdateModifiableProperties()
	{
		// Arrange
		var article = new Category(
			"initial Name",
			"initial Description"
		);

		// Act
		article.Update(
			"new Name",
			"new Description"
		);

		// Assert
		article.Name.Should().Be("new Name");
		article.Description.Should().Be("new Description");
		article.ModifiedOn.Should().NotBeNull("ModifiedOn should be set after update");
		article.ModifiedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
	}

	[Theory]
	[InlineData("", "description", "Name is required")]
	[InlineData("Name", "", "Description is required")]
	public void Category_WhenCreated_ShouldValidateRequiredFields(
		string name,
		string description,
		string expectedError)
	{
		// Arrange & Act & Assert
		FluentActions.Invoking(() => new Category(
			name,
			description
		)).Should().Throw<FluentValidation.ValidationException>()
			.WithMessage($"*{expectedError}*");
	}

	[Fact]
	public void Category_WhenUpdated_ShouldValidateRequiredFields()
	{
		// Arrange
		var article = new Category(
			"Name",
			"description"
		);

		// Act & Assert
		article.Invoking(a => a.Update(
			"",  // Empty Name should trigger validation
			"new description"
		)).Should().Throw<FluentValidation.ValidationException>()
			.WithMessage("*Name is required*");
	}

}