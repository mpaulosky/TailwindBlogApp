// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Category.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Entities;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Category))]
public class CategoryTests
{

	[Fact]
	public void Category_WhenCreated_ShouldHaveEmptyProperties()
	{
		// Arrange & Act
		var category = Category.Empty;

		// Assert
		category.Id.Should().Be(ObjectId.Empty);
		category.Name.Should().BeEmpty();
		category.Description.Should().BeEmpty();
		category.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
		category.ModifiedOn.Should().BeNull();
	}

	[Fact]
	public void Category_Empty_ShouldReturnEmptyInstance()
	{
		// Arrange & Act
		var category = Category.Empty;

		// Assert
		category.Id.Should().Be(ObjectId.Empty);
		category.Name.Should().BeEmpty();
		category.Description.Should().BeEmpty();
	}

	[Theory]
	[InlineData("Test Category", "Test Description")]
	[InlineData("Another Category", "Another Description")]
	public void Category_WhenPropertiesSet_ShouldHaveCorrectValues(string name, string description)
	{
		// Arrange
		var date = new DateTime(2025, 1, 1);

		var category = new Category(name, description);
		// Set CreatedOn and ModifiedOn if needed
		category.GetType().GetProperty("CreatedOn")?.SetValue(category, date);
		category.GetType().GetProperty("ModifiedOn")?.SetValue(category, date);

		// Assert
		category.Name.Should().Be(name);
		category.Description.Should().Be(description);
		category.CreatedOn.Should().Be(date);
		category.ModifiedOn.Should().Be(date);
	}

}
