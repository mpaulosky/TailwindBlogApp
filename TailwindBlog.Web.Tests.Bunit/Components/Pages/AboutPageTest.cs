// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AboutPageTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

namespace TailwindBlog.Web.Components.Pages;

/// <summary>
///   bUnit tests for the About page.
/// </summary>
public class AboutPageTest : BunitContext
{

	[Fact]
	public void AboutPage_Should_Render_Heading()
	{
		// Arrange & Act
		var cut = Render<About>();

		// Assert
		cut.Markup.Should().Contain("About");
	}

}
