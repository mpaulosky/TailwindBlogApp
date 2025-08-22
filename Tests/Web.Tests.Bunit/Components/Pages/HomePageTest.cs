// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     HomePageTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Pages;

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

		// Arrange
		const string expectedHtml =
				"""
				<header class="mx-auto max-w-7xl mb-6
								p-1 sm:px-4 md:px-6 lg:px-8 
								rounded-md shadow-md 
								shadow-blue-500">
				  <h1 class="text-3xl font-bold tracking-tight text-gray-50">Home - TailwindBlog</h1>
				</header>
				<h1>Hello, world!</h1>
				Welcome to your new app.
				""";

		// Act
		var cut = Render<Home>();

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

}