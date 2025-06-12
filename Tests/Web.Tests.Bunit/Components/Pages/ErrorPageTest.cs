// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ErrorPageTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Pages;

/// <summary>
///   bUnit tests for the Error page.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(Error))]
public class ErrorPageTest : BunitContext
{

	[Fact]
	public void ErrorPage_Should_Render_ErrorTitle()
	{
		// Arrange & Act
		var cut = Render<Error>();

		// Assert
		cut.Markup.Should().Contain("Error.");
		cut.Markup.Should().Contain("An error occurred while processing your request.");
	}

}
