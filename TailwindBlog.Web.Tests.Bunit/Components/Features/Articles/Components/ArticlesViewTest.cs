// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticlesViewTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

namespace TailwindBlog.Web.Components.Features.Articles.Components;

/// <summary>
///   bUnit tests for ArticlesView component.
/// </summary>
public class ArticlesViewComponentTest : BunitContext
{

	[Fact]
	public void Should_Render_Articles_And_Child_Components()
	{
		// Arrange & Act
		var cut = Render<ArticlesView>();

		// Assert
		cut.Markup.Should().Contain("font-bold text-xl mb-2 test-gray-800");
		cut.Markup.Should().Contain("Connect with us!");
		cut.Markup.Should().Contain("Recent Posts");
	}

}
