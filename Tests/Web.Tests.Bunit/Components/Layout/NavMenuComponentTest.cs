// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     NavMenuComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Layout;

using Microsoft.AspNetCore.Authorization;
using Bunit;

/// <summary>
///   bUnit tests for NavMenuComponent.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(NavMenuComponent))]
public class NavMenuComponentTest : BunitContext
{
	public NavMenuComponentTest()
	{
		this.Services.AddSingleton<IAuthorizationPolicyProvider, DefaultAuthorizationPolicyProvider>();
	}

	[Fact]
	public void Should_Render_NavMenu_Links()
	{
		// Arrange & Act
		var cut = this.Render<NavMenuComponent>();

		// Assert
		cut.Markup.Should().Contain("Articles");
		cut.Markup.Should().Contain("Contact");
		cut.Markup.Should().Contain("About");
	}
}