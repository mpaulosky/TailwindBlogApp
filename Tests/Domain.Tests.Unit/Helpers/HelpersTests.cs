// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     HelpersTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Helpers;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Helpers))]
public class HelpersTests
{

	[Fact]
	public void GetStaticDate_Should_Return_Expected_Date()
	{

		// Arrange & Act
		var staticDate = Helpers.GetStaticDate();
		
		staticDate.Should().Be(new DateTime(2025, 1, 1));

	}

	[Theory]
	[InlineData("Hello World!", "hello_world")]
	[InlineData("Test@Slug#123", "test_slug_123")]
	public void GetSlug_Should_Convert_To_Slug(string input, string expected)
	{

		// Arrange & Act
		var slug = input.GetSlug();

		// Assert
		slug.Should().Contain(expected);

	}

}