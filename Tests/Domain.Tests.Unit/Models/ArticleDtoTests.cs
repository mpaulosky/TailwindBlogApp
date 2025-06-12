// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleDtoTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Models;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(ArticleDto))]
public class ArticleDtoTests
{

	[Fact]
	public void ArticleDto_WhenCreated_ShouldHaveEmptyProperties()
	{

		// Arrange & Act
		var article = new ArticleDto();

		// Assert
		article.Id.Should().Be(ObjectId.Empty);
		article.Title.Should().BeEmpty();
		article.Introduction.Should().BeEmpty();
		article.Content.Should().BeEmpty();
		article.CoverImageUrl.Should().BeEmpty();
		article.UrlSlug.Should().BeEmpty();
		article.Author.Should().BeEquivalentTo(AppUserDto.Empty);
		article.Category.Should().BeEquivalentTo(CategoryDto.Empty);
		article.CreatedOn.Should().Be(DateTime.MinValue);
		article.ModifiedOn.Should().BeNull();
		article.IsPublished.Should().BeFalse();
		article.PublishedOn.Should().BeNull();

	}

	[Fact]
	public void ArticleDto_Empty_ShouldReturnEmptyInstance()
	{

		// Arrange & Act
		var article = ArticleDto.Empty;

		// Assert
		article.Id.Should().Be(ObjectId.Empty);
		article.Title.Should().BeEmpty();
		article.Introduction.Should().BeEmpty();
		article.Content.Should().BeEmpty();
		article.CoverImageUrl.Should().BeEmpty();
		article.UrlSlug.Should().BeEmpty();
		article.Author.Should().BeEquivalentTo(AppUserDto.Empty);
		article.Category.Should().BeEquivalentTo(CategoryDto.Empty);
		article.CreatedOn.Should().Be(DateTime.MinValue);
		article.ModifiedOn.Should().BeNull();
		article.IsPublished.Should().BeFalse();
		article.PublishedOn.Should().BeNull();

	}

	[Theory]
	[InlineData("Test Title", "Test Intro", "https://test.com/image.jpg", "test_title")]
	[InlineData("Another Title", "Another Intro", "https://test.com/another.jpg", "another_title")]
	public void ArticleDto_WhenPropertiesSet_ShouldHaveCorrectValues(
			string title,
			string introduction,
			string coverImageUrl,
			string urlSlug)
	{

		// Arrange & Act
		var now = DateTime.UtcNow;

		var article = new ArticleDto(
				ObjectId.GenerateNewId(),
				title,
				introduction,
				"This is the content.",
				coverImageUrl,
				urlSlug,
				AppUserDto.Empty,
				CategoryDto.Empty,
				now,
				null,
				false
		);

		// Assert
		article.Title.Should().Be(title);
		article.Introduction.Should().Be(introduction);
		article.CoverImageUrl.Should().Be(coverImageUrl);
		article.UrlSlug.Should().Be(urlSlug);
		article.Author.Should().BeEquivalentTo(AppUserDto.Empty);
		article.Category.Should().BeEquivalentTo(CategoryDto.Empty);
		article.CreatedOn.Should().Be(now);
		article.ModifiedOn.Should().BeNull();
		article.IsPublished.Should().BeFalse();
		article.PublishedOn.Should().BeNull();

	}

	[Theory]
	[InlineData("", "intro", "cover", "slug", "Title is required")]
	[InlineData("title", "", "cover", "slug", "Introduction is required")]
	[InlineData("title", "intro", "", "slug", "Cover image is required")]
	[InlineData("title", "intro", "cover", "", "URL slug is required")]
	public void ArticleDto_WhenCreated_ShouldValidateRequiredFields(
			string title,
			string introduction,
			string coverImageUrl,
			string urlSlug,
			string expectedError)
	{

		// Arrange & Act & Assert
		FluentActions.Invoking(() => new ArticleDto(
						ObjectId.GenerateNewId(),
						title,
						introduction,
						"Valid content.",
						coverImageUrl,
						urlSlug,
						AppUserDto.Empty,
						CategoryDto.Empty,
						DateTime.UtcNow,
						null,
						false
				)).Should().Throw<ValidationException>()
				.WithMessage($"*{expectedError}*");

	}

	[Fact]
	public void ArticleDto_WhenPublished_ShouldRequirePublishDate()
	{

		// Arrange & Act & Assert
		FluentActions.Invoking(() => new ArticleDto(
						ObjectId.GenerateNewId(),
						"title",
						"intro",
						"Valid content.",
						"cover",
						"slug",
						AppUserDto.Empty,
						CategoryDto.Empty,
						DateTime.UtcNow,
						null,
						true     // publishedOn missing should cause a validation error
				)).Should().Throw<ValidationException>()
				.WithMessage("*PublishedOn is required when IsPublished is true*");

	}

}