// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CollectionNamesTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================


namespace TailwindBlog.Domain.Helpers;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(CollectionNames))]
public class CollectionNamesTests
{

	[Theory]
	[InlineData("Article", "articles")]
	[InlineData("Category", "categories")]
	public void GetCollectionName_KnownEntities_ReturnsExpectedCollectionName(string entityName, string expected)
	{
		// Act
		var result = CollectionNames.GetCollectionName(entityName);

		// Assert
		result.Should().Be(expected);
	}

	[Theory]
	[InlineData("UnknownEntity")]
	[InlineData("")]
	[InlineData(null)]
	public void GetCollectionName_UnknownOrNullEntities_ReturnsEmptyString(string entityName)
	{
		// Act
		var result = CollectionNames.GetCollectionName(entityName);

		// Assert
		result.Should().BeEmpty();
	}

}