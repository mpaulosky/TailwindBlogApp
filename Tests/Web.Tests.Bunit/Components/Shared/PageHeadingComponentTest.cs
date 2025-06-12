// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     PageHeadingComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Shared;

/// <summary>
///   bUnit tests for PageHeadingComponent.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(PageHeadingComponent))]
public class PageHeadingComponentTest : BunitContext
{

	[Theory]
	[InlineData("1", "Test Heading 1", "text-3xl")]
	[InlineData("2", "Test Heading 2", "text-2xl")]
	[InlineData("3", "Test Heading 3", "text-1xl")]
	public void Should_Render_Correct_Heading_Level(string level, string headerText, string expectedHtml)
	{

		// Arrange & Act
		var cut = Render<PageHeadingComponent>(parameters => parameters
				.Add(p => p.Level, level)
				.Add(p => p.HeaderText, headerText));
		const string expectedHeader =
				"""
				<header class="mx-auto max-w-7xl mb-6
								px-4 py-4 sm:px-4 md:px-6 lg:px-8 
								rounded-md shadow-md 
								shadow-blue-500">
				""";
		
		// Assert
		cut.Markup.Should().Contain(headerText);
		cut.Markup.Should().Contain($"h{level}");
		cut.Markup.Should().Contain(expectedHeader);
		cut.Markup.Should().Contain(expectedHtml);
		
	}

}