// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ConnectWithUsComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

namespace TailwindBlog.Web.Components.Shared;

/// <summary>
///   bUnit tests for ConnectWithUsComponent.
/// </summary>
[ExcludeFromCodeCoverage]
public class ConnectWithUsComponentTest : BunitContext
{

	[Fact]
	public void Should_Render_ConnectWithUs_Title()
	{
		// Arrange & Act
		var cut = Render<ConnectWithUsComponent>();

		// Assert
		cut.Markup.Should().Contain("Connect with us!");
	}

}
