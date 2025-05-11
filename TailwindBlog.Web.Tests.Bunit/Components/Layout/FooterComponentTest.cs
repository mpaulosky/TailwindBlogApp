// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FooterComponentTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

#region

#endregion

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
		// Arrange & Act
		var cut = Render<FooterComponent>();

		// Assert
		cut.Markup.Should().Contain("All rights reserved");
	}

}
