// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FooterComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

namespace TailwindBlog.Web.Components.Layout;

/// <summary>
///   bUnit tests for FooterComponent.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(FooterComponent))]
public class FooterComponentTest : BunitContext
{

	[Fact]
	public void Should_Render_Footer_Text()
	{

		// Arrange
		const string expectedHtml =
				"""
				<div class="text-center px-6 py-2 mx-auto xl:max-w-5xl border-t-blue-700">
				  <a href="/">© 2023 MPaulosky Co. All rights reserved.</a>
				</div>
				""";

		// Act
		var cut = Render<FooterComponent>();

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

}