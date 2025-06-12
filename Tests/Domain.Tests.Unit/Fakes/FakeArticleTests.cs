// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FakeArticleTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Fakes;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(FakeArticle))]
public class FakeArticleTests
{

	[Fact]
	public void GetNewArticle_Should_Return_Article()
	{

		// Arrange & Act
		var article = FakeArticle.GetNewArticle();

		// Assert
		article.Should().NotBeNull();

	}

	[Fact]
	public void GetArticles_Should_Return_Correct_Count()
	{

		// Arrange & Act
		var articles = FakeArticle.GetArticles(2);

		// Assert
		articles.Should().HaveCount(2);

	}


}