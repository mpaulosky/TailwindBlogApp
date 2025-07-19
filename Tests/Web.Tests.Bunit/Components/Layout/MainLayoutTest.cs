// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     MainLayoutTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

using Microsoft.AspNetCore.Authorization;

namespace Web.Components.Layout;

/// <summary>
///   bUnit tests for MainLayout.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(MainLayout))]
public class MainLayoutTest : BunitContext
{
	public MainLayoutTest()
	{
		this.Services.AddSingleton<IAuthorizationPolicyProvider, DefaultAuthorizationPolicyProvider>();
	}

	[Fact]
	public void Should_Render_NavMenu_And_Footer()
	{
		// Arrange & Act
		var cut = this.Render<MainLayout>();

		// Assert
		cut.Markup.Should().Contain("Tailwind Blogs");
		cut.Markup.Should().Contain("All rights reserved");
	}
}