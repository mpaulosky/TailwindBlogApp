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
		var article = new Article
		{
				Author = AppUserModel.Empty
		};

		// Assert
		article.Id.Should().Be(ObjectId.Empty);
		article.Title.Should().BeEmpty();
		article.Introduction.Should().BeEmpty();
		article.CoverImageUrl.Should().BeEmpty();
		article.UrlSlug.Should().BeEmpty();
		article.Author.Id.Should().Be(AppUserModel.Empty.Id);
		article.Author.UserName.Should().Be(AppUserModel.Empty.UserName);
		article.Author.Email.Should().Be(AppUserModel.Empty.Email);
		article.Author.Roles.Should().BeEquivalentTo(AppUserModel.Empty.Roles);
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
		article.Author.Id.Should().Be(AppUserModel.Empty.Id);
		article.Author.UserName.Should().Be(AppUserModel.Empty.UserName);
		article.Author.Email.Should().Be(AppUserModel.Empty.Email);
		article.Author.Roles.Should().BeEquivalentTo(AppUserModel.Empty.Roles);
		article.IsPublished.Should().BeFalse();
		article.PublishedOn.Should().BeNull();
	}

	[Theory]
	[InlineData("Test Title", "Test Intro", "http://test.com/image.jpg", "test-slug")]
	[InlineData("Another Title", "Another Intro", "http://test.com/another.jpg", "another-slug")]
	public void Article_WhenPropertiesSet_ShouldHaveCorrectValues(
			string title,
			string introduction,
			string coverImageUrl,
			string urlSlug)
	{
		// Arrange
		var date = new DateTime(2025, 1, 1);

		var article = new Article
		{
				Title = title,
				Introduction = introduction,
				CoverImageUrl = coverImageUrl,
				UrlSlug = urlSlug,
				Author = AppUserModel.Empty,
				CreatedOn = date,
				ModifiedOn = date,
		};

		// Assert
		article.Title.Should().Be(title);
		article.Introduction.Should().Be(introduction);
		article.CoverImageUrl.Should().Be(coverImageUrl);
		article.UrlSlug.Should().Be(urlSlug);
		article.CreatedOn.Should().Be(date);
		article.ModifiedOn.Should().Be(date);
	}

	[Fact]
	public void Article_WhenPublished_ShouldSetPublishedProperties()
	{
		// Arrange
		var article = new Article
		{
				Author = AppUserModel.Empty,
				IsPublished = true,
				PublishedOn = DateTime.Now
		};

		// Assert
		article.IsPublished.Should().BeTrue();
		article.PublishedOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
	}

}
