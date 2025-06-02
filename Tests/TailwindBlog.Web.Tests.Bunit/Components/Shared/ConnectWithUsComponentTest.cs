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
[TestSubject(typeof(ConnectWithUsComponent))]
public class ConnectWithUsComponentTest : BunitContext
{

	[Fact]
	public void Should_Render_ConnectWithUs_Title()
	{
		// Arrange
		const string expectedHtml =
				"""
				<div>
				  <header class="mb-6 py-4">
				    <h1 class="text-2xl font-semibold tracking-tight py-4 text-gray-50">Connect With Us</h1>
				  </header>
				  <div class="flex flex-col gap-4">
				    <a href="https://www.threads/" target="_blank">
				      <i class="ri-threads-line text-3xl hover:bg-gray-100 hover:rounded p-2"></i>
				    </a>
				    <a href="https://www.instagram.com/" target="_blank">
				      <i class="ri-instagram-line text-3xl"></i>
				    </a>
				    <a href="https://www.youtube.com/" target="_blank">
				      <i class="ri-youtube-line text-3xl"></i>
				    </a>
				  </div>
				</div>
				""";

		// Act
		var cut = Render<ConnectWithUsComponent>();

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

}
