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

	private static readonly DateTime _staticDate = new(2025, 1, 1);

	[Fact]
	public void Category_WhenCreated_ShouldHaveEmptyProperties()
	{

		// Arrange & Act
		var category = new Category(
				string.Empty,
				true
		);

		// Assert
		category.Name.Should().BeEmpty();
		category.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromDays(1));
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
		category.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromDays(1));
		category.ModifiedOn.Should().BeNull();

	}

	[Theory]
	[InlineData("Test Name")]
	[InlineData("Another Name")]
	public void Category_WhenPropertiesSet_ShouldHaveCorrectValues(
			string name)
	{

		// Arrange & Act
		var category = FakeCategory.GetNewCategory(true);
		category.Name = name;
		category.ModifiedOn = null;

		// Assert
		category.Id.Should().NotBe(Guid.Empty);
		category.Name.Should().NotBeEmpty();
		category.Name.Should().Be(name);
		category.CreatedOn.Should().Be(_staticDate);
		category.ModifiedOn.Should().BeNull(); // Default value

	}

	[Fact]
	public void Category_Update_ShouldUpdateModifiableProperties()
	{

		// Arrange
		var category = FakeCategory.GetNewCategory(true);

		// Act
		category.Update(
				"new Name"
		);

		// Assert
		category.Id.Should().NotBe(Guid.Empty);
		category.Name.Should().Be("new Name");
		category.ModifiedOn.Should().NotBeNull("ModifiedOn should be set after update");
		category.ModifiedOn.Should().Be(_staticDate);

	}

	[Theory]
	[InlineData("", "Name is required.")]
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
	[InlineData("", "Name is required.")]
	public void Category_WhenUpdated_ShouldValidateRequiredFields(
			string name,
			string expectedError)
	{

		// Arrange
		var category = FakeCategory.GetNewCategory(true);
		category.Name = name;

		// Act & Assert
		category.Invoking(a => a.Update(
						name
				)).Should().Throw<ValidationException>()
				.WithMessage($"*{expectedError}*");

	}

}