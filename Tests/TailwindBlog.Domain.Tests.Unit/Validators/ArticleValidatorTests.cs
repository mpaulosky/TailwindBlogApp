// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleValidatorTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Validators;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(ArticleValidator))]
public class ArticleValidatorTests
{

	private readonly ArticleValidator _validator;

	public ArticleValidatorTests()
	{

		_validator = new ArticleValidator();

	}

	[Fact]
	public void Validate_WhenAllPropertiesValid_ShouldNotHaveErrors()
	{

		// Arrange
		var article = FakeArticle.GetNewArticle(true);

		// Act
		var result = _validator.Validate(article);

		// Assert
		result.IsValid.Should().BeTrue();
		result.Errors.Should().BeEmpty();

	}

	[Theory]
	[InlineData("", "intro", "content", "cover", "slug", "Title is required")]
	[InlineData("title", "", "content", "cover", "slug", "Introduction is required")]
	[InlineData("title", "intro", "", "cover", "slug", "Content is required")]
	[InlineData("title", "intro", "content", "", "slug", "Cover image is required")]
	[InlineData("title", "intro", "content", "cover", "", "URL slug is required")]
	public void Validate_WhenRequiredPropertyMissing_ShouldHaveError(
			string title,
			string introduction,
			string content,
			string coverImageUrl,
			string urlSlug,
			string expectedError)
	{

		// Arrange
		var article = new Article(
				title,
				introduction,
				content,
				coverImageUrl,
				urlSlug,
				AppUserDto.Empty,
				CategoryDto.Empty,
				false,
				null,
				true
		);

		// Act
		var result = _validator.Validate(article);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.ErrorMessage == expectedError);

	}

	[Theory]
	[InlineData("title", "intro", "content", "cover", "invalid slug",
			"URL slug can only contain lowercase letters, numbers, and underscores")]
	[InlineData("title", "intro", "content", "cover", "Invalid-Slug",
			"URL slug can only contain lowercase letters, numbers, and underscores")]
	[InlineData("title", "intro", "content", "cover", "invalid.slug",
			"URL slug can only contain lowercase letters, numbers, and underscores")]
	public void Validate_WhenUrlSlugInvalid_ShouldHaveError(
			string title,
			string introduction,
			string content,
			string coverImageUrl,
			string urlSlug,
			string expectedError)
	{

		// Arrange
		var article = new Article(
				title,
				introduction,
				content,
				coverImageUrl,
				urlSlug,
				AppUserDto.Empty,
				CategoryDto.Empty,
				false,
				null,
				true
		);

		// Act
		var result = _validator.Validate(article);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.ErrorMessage == expectedError);

	}

	[Fact]
	public void Validate_WhenPublishedWithoutPublishDate_ShouldHaveError()
	{

		// Arrange
		var article = new Article(
				"title",
				"intro",
				"content",
				"cover",
				"slug",
				AppUserDto.Empty,
				CategoryDto.Empty,
				true, // IsPublished
				null, // PublishedOn missing
				true
		);

		// Act
		var result = _validator.Validate(article);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.ErrorMessage == "PublishedOn is required when IsPublished is true");

	}

	[Fact]
	public void Validate_WhenContentTooLong_ShouldHaveError()
	{

		// Arrange
		var article = new Article(
				"title",
				"intro",
				new string('x', 4001), // Content too long
				"cover",
				"slug",
				AppUserDto.Empty,
				CategoryDto.Empty,
				false,
				null,
				true
		);

		// Act
		var result = _validator.Validate(article);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Content cannot exceed 4000 characters");

	}

}