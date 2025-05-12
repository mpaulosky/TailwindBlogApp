// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     PostInfoComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

#region

using System;

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

		var category = "UnitTest";

		// Act
		var cut = Render<PostInfoComponent>(parameters => parameters
				.Add(p => p.Article, article)
				.Add(p => p.CategoryType, category));

		// Assert
		cut.Markup.Should().Contain("TestUser");
		cut.Markup.Should().Contain("UnitTest");
		cut.Markup.Should().Contain("Published: 5/4/2025");
	}

}
