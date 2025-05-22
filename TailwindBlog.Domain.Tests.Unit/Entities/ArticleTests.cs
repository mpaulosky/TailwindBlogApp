// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Article.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Entities;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Article))]
public class ArticleTests
{
	[Fact]
	public void Article_WhenCreated_ShouldHaveEmptyProperties()
	{
		// Arrange & Act
		var article = new Article(
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			AppUserModel.Empty,
			skipValidation: true
		);

		// Assert
		article.Title.Should().BeEmpty();
		article.Introduction.Should().BeEmpty();
		article.CoverImageUrl.Should().BeEmpty();
		article.UrlSlug.Should().BeEmpty();
		article.Author.Should().BeEquivalentTo(AppUserModel.Empty);
		article.IsPublished.Should().BeFalse();
		article.PublishedOn.Should().BeNull();
		article.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
		article.ModifiedOn.Should().BeNull();
	}

	[Fact]
	public void Article_Empty_ShouldReturnEmptyInstance()
	{
		// Arrange & Act
		var article = Article.Empty;

		// Assert
		article.Id.Should().Be(ObjectId.Empty);
		article.Title.Should().BeEmpty();
		article.Introduction.Should().BeEmpty();
		article.CoverImageUrl.Should().BeEmpty();
		article.UrlSlug.Should().BeEmpty();
		article.Author.Should().BeEquivalentTo(AppUserModel.Empty);
		article.IsPublished.Should().BeFalse();
		article.PublishedOn.Should().BeNull();
		// Note: Don't test CreatedOn/ModifiedOn for Empty instance as they are init properties
	}

	[Theory]
	[InlineData("Test Title", "Test Intro", "https://test.com/image.jpg", "test-slug")]
	[InlineData("Another Title", "Another Intro", "https://test.com/another.jpg", "another-slug")]
	public void Article_WhenPropertiesSet_ShouldHaveCorrectValues(
		string title,
		string introduction,
		string coverImageUrl,
		string urlSlug)
	{
		// Arrange & Act
		var article = new Article(
			title,
			introduction,
			coverImageUrl,
			urlSlug,
			AppUserModel.Empty
		);

		// Assert
		article.Title.Should().Be(title);
		article.Introduction.Should().Be(introduction);
		article.CoverImageUrl.Should().Be(coverImageUrl);
		article.UrlSlug.Should().Be(urlSlug);
		article.Author.Should().BeEquivalentTo(AppUserModel.Empty);
		article.IsPublished.Should().BeFalse(); // Default value
		article.PublishedOn.Should().BeNull(); // Default value
		article.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
		article.ModifiedOn.Should().BeNull(); // Default value
	}

	[Fact]
	public void Article_WhenPublished_ShouldSetPublishedProperties()
	{
		// Arrange
		var now = DateTime.Now;
		var author = new AppUserModel { UserName = "test user", Email = "test@example.com" };

		var article = new Article(
			"title",
			"introduction",
			"coverImageUrl",
			"urlSlug",
			author,
			true,
			now
		);

		// Assert
		article.IsPublished.Should().BeTrue();
		article.PublishedOn.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
		article.Author.Should().BeEquivalentTo(author);
		article.CreatedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
	}

	[Fact]
	public void Article_Update_ShouldUpdateModifiableProperties()
	{
		// Arrange
		var article = new Article(
			"initial title",
			"initial intro",
			"initial cover",
			"initial-slug",
			AppUserModel.Empty
		);

		var newAuthor = new AppUserModel { UserName = "new user", Email = "new@example.com" };
		var publishDate = DateTime.Now;

		// Act
		article.Update(
			"new title",
			"new intro",
			"new cover",
			"new-slug",
			newAuthor,
			true,
			publishDate
		);

		// Assert
		article.Title.Should().Be("new title");
		article.Introduction.Should().Be("new intro");
		article.CoverImageUrl.Should().Be("new cover");
		article.UrlSlug.Should().Be("new-slug");
		article.Author.Should().BeEquivalentTo(newAuthor);
		article.IsPublished.Should().BeTrue();
		article.PublishedOn.Should().BeCloseTo(publishDate, TimeSpan.FromSeconds(1));
		article.ModifiedOn.Should().NotBeNull("ModifiedOn should be set after update");
		article.ModifiedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
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
			coverImageUrl,
			urlSlug,
			AppUserModel.Empty
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
			"cover",
			"slug",
			AppUserModel.Empty
		);

		// Act & Assert
		article.Invoking(a => a.Update(
			"",  // Empty title should trigger validation
			"new intro",
			"new cover",
			"new-slug",
			AppUserModel.Empty,
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
				"cover",
				"slug",
				AppUserModel.Empty,
				true   // But no publication date
			)).Should().Throw<FluentValidation.ValidationException>()
			.WithMessage("*PublishedOn is required when IsPublished is true*");
	}
}