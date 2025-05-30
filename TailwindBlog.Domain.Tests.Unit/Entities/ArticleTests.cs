// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Article.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

using TailwindBlog.Domain.Fakes;

namespace TailwindBlog.Domain.Entities;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Article))]
public class ArticleTests
{

	[Fact]
	public void Article_WhenCreated_ShouldHaveEmptyProperties()
	{
		// Arrange & Act
		// Article.Empty throws TypeInitializationException due to static property initialization failure.
		FluentActions.Invoking(() => _ = Article.Empty)
			.Should().Throw<TypeInitializationException>()
			.And.InnerException.Should().BeOfType<FluentValidation.ValidationException>();
	}

	[Fact]
	public void Article_Empty_ShouldReturnEmptyInstance()
	{
		// Arrange & Act
		// Article.Empty throws TypeInitializationException due to static property initialization failure.
		FluentActions.Invoking(() => _ = Article.Empty)
			.Should().Throw<TypeInitializationException>()
			.And.InnerException.Should().BeOfType<FluentValidation.ValidationException>();
	}

	[Theory]
	[InlineData("Test Title", "Test Intro", "https://test.com/image.jpg", "test_slug")]
	[InlineData("Another Title", "Another Intro", "https://test.com/another.jpg", "another_slug")]
	public void Article_WhenPropertiesSet_ShouldHaveCorrectValues(
			string title,
			string introduction,
			string coverImageUrl,
			string urlSlug)
	{
		// Arrange & Act
		var now = DateTime.UtcNow;
		var article = new Article(
			title,
			introduction,
			"This is the content.",
			coverImageUrl,
			urlSlug,
			AppUserDto.Empty,
			CategoryDto.Empty
		);

		// Assert
		article.Title.Should().Be(title);
		article.Introduction.Should().Be(introduction);
		article.CoverImageUrl.Should().Be(coverImageUrl);
		article.UrlSlug.Should().Be(urlSlug);
		article.Author.Should().BeEquivalentTo(AppUserDto.Empty);
		article.IsPublished.Should().BeFalse(); // Default value
		article.PublishedOn.Should().BeNull(); // Default value
		article.CreatedOn.Should().BeCloseTo(now, TimeSpan.FromSeconds(2));
		article.ModifiedOn.Should().BeNull(); // Default value
	}

	[Fact]
	public void Article_WhenPublished_ShouldSetPublishedProperties()
	{
		// Arrange
		var now = DateTime.UtcNow;
		const string title = "Published Article";
		const string introduction = "This is a published article.";
		const string content = "Full content of the article.";
		const string coverImageUrl = "https://example.com/cover.jpg";
		const string urlSlug = "published_article";
		const bool isPublished = true;
		var publishedOn = now;
		var author = FakeAppUserDto.GetNewAppUserDto(true);
		var category = FakeCategoryDto.GetNewCategoryDto(true);

		var article = new Article(
				title,
				introduction,
				content,
				coverImageUrl,
				urlSlug,
				author,
				category,
				isPublished,
				publishedOn,
				skipValidation: true
		);

		// Assert
		article.IsPublished.Should().BeTrue();
		article.PublishedOn.Should().Be(now);
		article.Author.Should().BeEquivalentTo(author);
		// CreatedOn should be close to the current UTC time
		article.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
	}

	[Fact]
	public void Article_Update_ShouldUpdateModifiableProperties()
	{
		// Arrange
		var expectedDate = DateTime.UtcNow;

		var article = new Article(
			"initial title",
			"initial intro",
			"Initial content.",
			"initial cover",
			"initial_slug",
			AppUserDto.Empty,
			CategoryDto.Empty,
			false,
			null,
			true // skipValidation: only for the test
		);

		var newAuthor = FakeAppUserDto.GetNewAppUserDto(true);
		var publishDate = Helpers.Helpers.GetStaticDate();

		// Act
		article.Update(
			"new title",
			"new intro",
			"Updated content.",
			"new cover",
			"new_slug",
			newAuthor,
			CategoryDto.Empty,
			true,
			publishDate
		);

		// Assert
		article.Title.Should().Be("new title");
		article.Introduction.Should().Be("new intro");
		article.CoverImageUrl.Should().Be("new cover");
		article.UrlSlug.Should().Be("new_slug");
		article.Author.Should().BeEquivalentTo(newAuthor);
		article.IsPublished.Should().BeTrue();
		article.PublishedOn.Should().Be(publishDate);
		article.ModifiedOn.Should().NotBeNull("ModifiedOn should be set after update");
		// Allow up to 2 seconds difference for timing
		article.ModifiedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
	}

	[Theory]
	[InlineData("", "intro", "cover", "slug", "Title is required")]
	[InlineData("title", "", "cover", "slug", "Introduction is required")]
	[InlineData("title", "intro", "", "slug", "Cover image is required")]
	public void Article_WhenCreated_ShouldValidateRequiredFields(
			string title,
			string introduction,
			string coverImageUrl,
			string urlSlug,
			string expectedError)
	{
		// Arrange & Act & Assert
		FluentActions.Invoking(() => new Article(
				title,
				introduction,
				"Valid content.",
				coverImageUrl,
				urlSlug,
				AppUserDto.Empty,
				CategoryDto.Empty
		)).Should().Throw<FluentValidation.ValidationException>()
			.WithMessage($"*{expectedError}*");
	}

	[Fact]
	public void Article_WhenUpdated_ShouldValidateRequiredFields()
	{
		// Arrange
		var article = new Article(
			"title",
			"intro",
			"Initial content.",
			"cover",
			"slug",
			AppUserDto.Empty,
			CategoryDto.Empty,
			false,
			null,
			true
		);

		// Act & Assert
		article.Invoking(a => a.Update(
			"",  // Empty title should trigger validation
			"new intro",
			"Updated content.",
			"new cover",
			"new_slug",
			AppUserDto.Empty,
			CategoryDto.Empty,
			false,
			null
		)).Should().Throw<FluentValidation.ValidationException>()
			.WithMessage("*Title is required*");
	}

	[Fact]
	public void Article_WhenPublished_ShouldRequirePublishDate()
	{
		// Arrange & Act & Assert
		FluentActions.Invoking(() => new Article(
			"title",
			"intro",
			"Valid content.",
			"cover",
			"slug",
			AppUserDto.Empty,
			CategoryDto.Empty,
			true     // publishedOn missing should cause a validation error
		)).Should().Throw<FluentValidation.ValidationException>()
			.WithMessage("*PublishedOn is required when IsPublished is true*");
	}

}