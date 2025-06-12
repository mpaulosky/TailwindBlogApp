// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FakeCategoryTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Fakes;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(FakeCategory))]
public class FakeCategoryTests
{

	[Fact]
	public void GetNewCategory_ShouldReturnCategory()
	{

		// Act
		var result = FakeCategory.GetNewCategory();

		// Assert
		result.Should().NotBeNull();
		result.Should().BeOfType<Category>();
		result.Name.Should().NotBeNullOrEmpty();
		result.Description.Should().NotBeNullOrEmpty();

	}

	[Fact]
	public void GetNewCategory_WithSeed_ShouldReturnDeterministicResult()
	{

		// Act
		var result1 = FakeCategory.GetNewCategory(useSeed: true);
		var result2 = FakeCategory.GetNewCategory(useSeed: true);

		// Assert
		result1.Should().NotBeNull();
		result2.Should().NotBeNull();
		result1.Name.Should().Be(result2.Name);
		result1.Description.Should().Be(result2.Description);

	}

	[Theory]
	[InlineData(1)]
	[InlineData(5)]
	[InlineData(10)]
	public void GetCategories_ShouldReturnRequestedNumberOfCategories(int count)
	{

		// Act
		var results = FakeCategory.GetCategories(count);

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(count);
		results.Should().AllBeOfType<Category>();
		results.Should().OnlyContain(c => !string.IsNullOrEmpty(c.Name));
		results.Should().OnlyContain(c => !string.IsNullOrEmpty(c.Description));

	}

	[Fact]
	public void GetCategories_WithSeed_ShouldReturnDeterministicResults()
	{

		// Arrange
		const int count = 3;

		// Act
		var results1 = FakeCategory.GetCategories(count, useSeed: true);
		var results2 = FakeCategory.GetCategories(count, useSeed: true);

		// Assert
		results1.Should().NotBeNull();
		results2.Should().NotBeNull();
		results1.Should().HaveCount(count);
		results2.Should().HaveCount(count);

		for (int i = 0; i < count; i++)
		{
			results1[i].Name.Should().Be(results2[i].Name);
			results1[i].Description.Should().Be(results2[i].Description);
		}

	}

	[Fact]
	public void GenerateFake_ShouldConfigureFakerCorrectly()
	{

		// Act
		var faker = FakeCategory.GenerateFake();
		var category = faker.Generate();

		// Assert
		category.Should().NotBeNull();
		category.Name.Should().NotBeNullOrEmpty();
		category.Name.Should().BeOneOf(Enum.GetNames<CategoryNames>());
		category.Description.Should().Contain(category.Name);

	}

	[Fact]
	public void GenerateFake_WithSeed_ShouldApplySeed()
	{

		// Act
		var faker1 = FakeCategory.GenerateFake(useSeed: true);
		var faker2 = FakeCategory.GenerateFake(useSeed: true);

		var category1 = faker1.Generate();
		var category2 = faker2.Generate();

		// Assert
		category1.Name.Should().Be(category2.Name);
		category1.Description.Should().Be(category2.Description);

	}

}