// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     IApplicationDbContextTests.cs
// Company :       mpaulosky
// Author :        GitHub Copilot
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

using MongoDB.Driver;

namespace Domain.Interfaces;

/// <summary>
///   Unit tests for IApplicationDbContext interface contract.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(IApplicationDbContext))]
public class IApplicationDbContextTests
{

	[Fact]
	public void IApplicationDbContext_ShouldExposeArticlesAndCategoriesCollections()
	{
		// Arrange
		var dbContext = Substitute.For<IApplicationDbContext>();
		var articles = Substitute.For<IMongoCollection<Article>>();
		var categories = Substitute.For<IMongoCollection<Category>>();
		dbContext.Articles.Returns(articles);
		dbContext.Categories.Returns(categories);

		// Act & Assert
		dbContext.Articles.Should().NotBeNull();
		dbContext.Categories.Should().NotBeNull();
	}

}