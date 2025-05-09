// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ContactPageTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web.Tests.Bunit
// =======================================================

#region

#endregion

namespace TailwindBlog.Web.Components.Pages;

/// <summary>
///   bUnit tests for the Contact page.
/// </summary>
public class ContactPageTest : BunitContext
{

	[Fact]
	public void ContactPage_Should_Render_Heading()
	{
		// Arrange & Act
		var cut = Render<Contact>();

		// Assert
		cut.Markup.Should().Contain("Contacts");
	}

}
