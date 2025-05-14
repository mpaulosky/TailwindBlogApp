// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Category.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

using System.ComponentModel.DataAnnotations;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
using FluentAssertions;
using TailwindBlog.Domain.Entities;
using TailwindBlog.Domain.Extensions;

namespace TailwindBlog.Domain.Tests.Unit.Entities;

public class CategoryTests
{
	[Fact]
	public void Category_WhenCreated_ShouldHaveEmptyProperties()
	{
		// Arrange & Act
		var category = new Category(
				string.Empty,
				string.Empty
		);

		// Assert
		category.Id.Should().NotBeEmpty();
		category.Name.Should().BeEmpty();
		category.Description.Should().BeEmpty();
		category.UrlSlug.Should().BeEmpty();
		category.ShowOnNavigation.Should().BeFalse();
		category.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
		category.ModifiedOn.Should().BeNull();
	}

	[Fact]
	public void Category_Empty_ShouldReturnEmptyInstance()
	{
		// Arrange & Act
		var category = Category.Empty;

		// Assert
		category.Id.Should().Be(Guid.Empty);
		category.Name.Should().BeEmpty();
		category.Description.Should().BeEmpty();
		category.UrlSlug.Should().BeEmpty();
		category.ShowOnNavigation.Should().BeFalse();
		// Note: Don't test CreatedOn/ModifiedOn for Empty instance as they are init properties
	}

	[Theory]
	[InlineData("Test Category", "Test Description")]
	[InlineData("Another Category", "Another Description")]
	public void Category_WhenPropertiesSet_ShouldHaveCorrectValues(string name, string description)
	{
		// Arrange & Act
		var category = new Category(name, description);

		// Assert
		category.Name.Should().Be(name);
		category.Description.Should().Be(description);
		category.UrlSlug.Should().BeEmpty();
		category.ShowOnNavigation.Should().BeFalse();
		category.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
		category.ModifiedOn.Should().BeNull();
	}

	[Theory]
	[InlineData("", "description", "Name is required")]
	[InlineData("name", "", "Description is required")]
	public void Category_WhenCreated_ShouldValidateRequiredFields(
			string name,
			string description,
			string expectedError)
	{
		// Arrange & Act
		var category = new Category(name, description);

		// Assert
		var context = new ValidationContext(category);
		var validationResults = new List<ValidationResult>();
		var isValid = Validator.TryValidateObject(category, context, validationResults, true);

		isValid.Should().BeFalse();
		validationResults.Should().Contain(v => v.ErrorMessage == expectedError);
	}

	[Fact]
	public void Category_WhenUpdated_ShouldUpdateModifiableProperties()
	{
		// Arrange
		var category = new Category(
				"initial name",
				"initial description"
		);

		// Act
		category.Update(
				"new name",
				"new description"
		);

		// Assert
		category.Name.Should().Be("new name");
		category.Description.Should().Be("new description");
		category.ModifiedOn.Should().NotBeNull("ModifiedOn should be set after update");
		category.ModifiedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
	}

	[Fact]
	public void Category_WhenUpdated_ShouldUpdateNavigationFlag()
	{
		// Arrange
		var category = new Category(
				"initial name",
				"initial description"
		);

		// Act
		category.Update(
				"new name",
				"new description",
				true // show on navigation
		);

		// Assert
		category.Name.Should().Be("new name");
		category.Description.Should().Be("new description");
		category.ShowOnNavigation.Should().BeTrue();
		category.ModifiedOn.Should().NotBeNull();
		category.ModifiedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
	}

	[Fact]
	public void Category_WhenUpdated_ShouldUpdateUrlSlug()
	{
		// Arrange
		var category = new Category(
				"initial name",
				"initial description"
		);

		// Act
		category.Update(
				"new name",
				"new description",
				"new-slug",
				true // show on navigation
		);

		// Assert
		category.Name.Should().Be("new name");
		category.Description.Should().Be("new description");
		category.UrlSlug.Should().Be("new-slug");
		category.ShowOnNavigation.Should().BeTrue();
		category.ModifiedOn.Should().NotBeNull();
		category.ModifiedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
	}

	[Theory]
	[InlineData("", "new description", "Name is required")]
	[InlineData("new name", "", "Description is required")]
	public void Category_WhenUpdated_ShouldValidateRequiredFields(
			string name,
			string description,
			string expectedError)
	{
		// Arrange
		var category = new Category(
				"initial name",
				"initial description"
		);

		// Act & Assert
		category.Invoking(c => c.Update(name, description))
				.Should().Throw<FluentValidation.ValidationException>()
				.WithMessage($"{expectedError}");
	}
}
