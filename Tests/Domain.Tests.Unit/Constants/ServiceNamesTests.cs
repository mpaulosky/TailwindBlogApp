// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ServiceNamesTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Constants;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(ServiceNames))]
public class ServiceNamesTests
{

	[Fact]
	public void ServerName_ShouldReturnExpectedValue()
	{

		// Arrange
		var expected = "posts-server";

		// Act
		var actual = ServiceNames.ServerName;

		// Assert
		Assert.Equal(expected, actual);

	}

	[Fact]
	public void DatabaseName_ShouldReturnExpectedValue()
	{

		// Arrange
		var expected = "articlesdb";

		// Act
		var actual = ServiceNames.DatabaseName;

		// Assert
		Assert.Equal(expected, actual);

	}

	[Fact]
	public void OutputCache_ShouldReturnExpectedValue()
	{

		// Arrange
		var expected = "output-cache";

		// Act
		var actual = ServiceNames.OutputCache;

		// Assert
		Assert.Equal(expected, actual);

	}

	[Fact]
	public void ApiService_ShouldReturnExpectedValue()
	{

		// Arrange
		var expected = "blog-api";

		// Act
		var actual = ServiceNames.ApiService;

		// Assert
		Assert.Equal(expected, actual);

	}

	[Fact]
	public void WebApp_ShouldReturnExpectedValue()
	{

		// Arrange
		var expected = "web-frontend";

		// Act
		var actual = ServiceNames.WebApp;

		// Assert
		Assert.Equal(expected, actual);

	}

	[Fact]
	public void CategoryCacheName_ShouldReturnExpectedValue()
	{

		// Arrange
		var expected = "CategoryData";

		// Act
		var actual = ServiceNames.CategoryCacheName;

		// Assert
		Assert.Equal(expected, actual);

	}

	[Fact]
	public void BlogPostCacheName_ShouldReturnExpectedValue()
	{

		// Arrange
		var expected = "BlogPostData";

		// Act
		var actual = ServiceNames.BlogPostCacheName;

		// Assert
		Assert.Equal(expected, actual);

	}

}