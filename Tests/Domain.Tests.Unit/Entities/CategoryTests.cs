// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Categories.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Category))]
public class CategoryTests
{

	[Fact]
	public void Category_WhenCreated_ShouldHaveEmptyProperties()
	{

		// Arrange & Act
		var article = new Category(
				string.Empty,
				true
		);

		// Assert
		article.Name.Should().BeEmpty();
		article.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromDays(1));
		article.ModifiedOn.Should().BeNull();

	}

	[Fact]
	public void Category_Empty_ShouldReturnEmptyInstance()
	{

		// Arrange & Act
		var article = Category.Empty;

		// Assert
		article.Id.Should().Be(Guid.Empty);
		article.Name.Should().BeEmpty();

	}

	[Theory]
	[InlineData("Test Name")]
	[InlineData("Another Name")]
	public void Category_WhenPropertiesSet_ShouldHaveCorrectValues(
			string name)
	{

		// Arrange & Act
		var article = new Category(
				name
		);

		// Assert
		article.Name.Should().Be(name);
		article.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromDays(1));
		article.ModifiedOn.Should().BeNull(); // Default value

	}

	[Fact]
	public void Category_Update_ShouldUpdateModifiableProperties()
	{

		// Arrange
		var article = new Category(
				"initial Name"
		);

		// Act
		article.Update(
				"new Name"
		);

		// Assert
		article.Name.Should().Be("new Name");
		article.ModifiedOn.Should().NotBeNull("ModifiedOn should be set after update");
		article.ModifiedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));

	}

	[Theory]
	[InlineData("", "Name is required")]
	public void Category_WhenCreated_ShouldValidateRequiredFields(
			string name,
			string expectedError)
	{

		// Arrange & Act & Assert
		FluentActions.Invoking(() => new Category(
						name
				)).Should().Throw<ValidationException>()
				.WithMessage($"*{expectedError}*");

	}

	[Theory]
	[InlineData("", "Name is required")]
	public void Category_WhenUpdated_ShouldValidateRequiredFields(
			string name,
			string expectedError)
	{

		// Arrange
		var article = new Category(
				"Name"
		);

		// Act & Assert
		article.Invoking(a => a.Update(
						name
				)).Should().Throw<ValidationException>()
				.WithMessage($"*{expectedError}*");

	}

}