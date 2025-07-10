// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     NavMenuComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Layout;

/// <summary>
///   bUnit tests for NavMenuComponent.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(NavMenuComponent))]
public class NavMenuComponentTest : BunitContext
{

	[Fact]
	public void Should_Render_NavMenu_Links()
	{
		// Arrange & Act
		var cut = Render<NavMenuComponent>();

		// Assert
		cut.Markup.Should().Contain("Articles");
		cut.Markup.Should().Contain("Contact");
		cut.Markup.Should().Contain("About");
	}

}