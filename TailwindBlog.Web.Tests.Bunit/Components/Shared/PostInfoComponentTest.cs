// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     PostInfoComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

#region

using TailwindBlog.Domain.Models;
using TailwindBlog.Web.Components.Features.Articles.Models;

#endregion

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

		var article = new ArticleModel
		{
				Author = new AppUserModel { UserName = "TestUser" },
				CreatedOn = new DateTime(2025, 5, 5),
				IsPublished = true,
				PublishedOn = new DateTime(2025, 5, 4)
		};

		const string expectedHtml =
				"""
				<div class="flex gap-4 border-t border-gray-200 pt-4">
				  <div>Author: TestUser</div>
				  <div>Created: 5/5/2025</div>
				  <div>Published: 5/4/2025</div>
				  <div>Category: UnitTest</div>
				</div>
				""";


		const string category = "UnitTest";

		// Act
		var cut = Render<PostInfoComponent>(parameters => parameters
				.Add(p => p.Article, article)
				.Add(p => p.CategoryType, category));

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

}