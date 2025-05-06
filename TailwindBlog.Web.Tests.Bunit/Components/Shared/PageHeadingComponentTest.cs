// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     PageHeadingComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

namespace TailwindBlog.Web.Components.Shared;

/// <summary>
///   bUnit tests for PageHeadingComponent.
/// </summary>
[ExcludeFromCodeCoverage]
public class PageHeadingComponentTest : BunitContext
{

	[Theory]
	[InlineData("1", "Test Heading",
			"<h1 class=\"text-2xl font-bold tracking-tight text-gray-50 py-4\">Test Heading</h1>")]
	[InlineData("2", "Test Heading",
			"<h2 class=\"text-3xl font-bold tracking-tight text-gray-50 py-4\">Test Heading</h2>")]
	[InlineData("3", "Test Heading",
			"<h3 class=\"text-2xl font-bold tracking-tight text-gray-50 py-4\">Test Heading</h3>")]
	public void Should_Render_Correct_Heading_Level(string level, string headerText, string expectedHtml)
	{
		// Arrange & Act
		var cut = Render<PageHeadingComponent>(parameters => parameters
				.Add(p => p.Level, level)
				.Add(p => p.HeaderText, headerText));

		// Assert
		cut.Markup.Should().Contain(headerText);
		cut.Markup.Should().Contain(expectedHtml);
	}

}
