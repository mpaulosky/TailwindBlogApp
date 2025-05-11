// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     HomePageTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

namespace TailwindBlog.Web.Components.Pages;

/// <summary>
///   bUnit tests for the Home page.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(Home))]
public class HomePageTest : BunitContext
{

	[Fact]
	public void HomePage_Should_Render_HelloWorld()
	{
		var cut = Render<Home>();

		// Assert
		cut.Markup.Should().Contain("Hello, world!");
	}

}
