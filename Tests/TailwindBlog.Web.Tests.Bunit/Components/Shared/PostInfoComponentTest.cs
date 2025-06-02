// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     PostInfoComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

namespace TailwindBlog.Web.Components.Shared;

/// <summary>
///   bUnit tests for PostInfoComponent.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(PostInfoComponent))]
public class PostInfoComponentTest : BunitContext
{

	[Fact]
	public void Should_Render_Author_And_Category()
	{
		// Arrange
		var article = FakeArticle.GetNewArticle(true).Adapt<ArticleModel>();
		article.Author.UserName = "TestUser";
		article.CreatedOn = new DateTime(2025, 5, 5);
		article.PublishedOn = new DateTime(2025, 5, 4);
		article.Category.Name = "UnitTest";

		const string expectedHtml =
				"""
				<div class="flex gap-4 border-t border-gray-200 pt-4">
				  <div>Author: TestUser</div>
				  <div>Created: 5/5/2025</div>
				  <div>Published: 5/4/2025</div>
				  <div>Category: UnitTest</div>
				</div>
				""";

		// Act
		var cut = Render<PostInfoComponent>(parameters => parameters
				.Add(p => p.Article, article));

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

}