// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     NotAuthorizedComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

namespace TailwindBlog.Web.Components.Shared;

/// <summary>
///   bUnit tests for NotAuthorizedComponent.
/// </summary>
[ExcludeFromCodeCoverage]
public class NotAuthorizedComponentTest : BunitContext
{

	[Fact]
	public void Should_Render_NotAuthorized_Message()
	{
		// Arrange & Act
		var cut = Render<NotAuthorizedComponent>();

		// Assert
		cut.Markup.Should().Contain("not authorized for this page");
	}

}
