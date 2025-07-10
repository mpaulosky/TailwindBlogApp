// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticlesViewTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Features.Articles;

/// <summary>
///   bUnit tests for ArticlesView component.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(ArticlesView))]
public class ArticlesViewComponentTest : BunitContext
{

	[Fact]
	public void Should_Render_Articles_And_Child_Components()
	{
		// Arrange
		const string expectedHtml =
				"""
				<div class="grid grid-cols-1 lg:grid-cols-3 mt-6">
					<div class="lg:col-span-2 text-gray-60 bg-gray-800">
						<div class="rounded-2xl shadow-md hover:shadow-blue-500">
							<article class="md:grid md:grid-cols-1 md:items-baseline" diff:ignoreChildren></article>
						</div>
						<div class="rounded-2xl shadow-md hover:shadow-blue-500">
							<article class="md:grid md:grid-cols-1 md:items-baseline" diff:ignoreChildren></article>
						</div>
						<div class="rounded-2xl shadow-md hover:shadow-blue-500">
							<article class="md:grid md:grid-cols-1 md:items-baseline" diff:ignoreChildren></article>
						</div>
						}
					</div>
					<div class="pl-6 pt-12 lg:pt-4">
						<header class="mb-6 py-4">
							<h1 class="text-2xl font-semibold tracking-tight py-4 text-gray-50">Connect With Us</h1>
						</header>
						<div>
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
						<div>
							<header class="mb-6 py-4">
								<h1 class="text-2xl font-semibold tracking-tight py-4 text-gray-50">Recent Posts</h1>
							</header>
							<div class="flex flex-col gap-4">
								<div>
									<a href="/run-sql-server-on-m1-or-m2-macbook" target="_blank" class="hover:text-blue-700">
										Run SQL Server on M1 or M2 Macbook
									</a>
								</div>
								<div>
									<a href="/run-sql-server-on-m1-or-m2-macbook" target="_blank" class="hover:text-blue-700">
										Run a Postgres Database for Free on Google Cloud
									</a>
								</div>
							</div>
						</div>
					</div>
				</div>
				""";

		// Act
		var cut = Render<ArticlesView>();

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

	/// <summary>
	///   Validates that the ConnectWithUsComponent renders the correct social links and icons.
	/// </summary>
	[Fact]
	public void ConnectWithUsComponent_Should_Render_Valid_Data()
	{

		// Arrange
		var cut = Render<ArticlesView>();

		// Act
		var connectSection = cut.Find("div.pl-6.pt-12.lg\\:pt-4");
		var links = connectSection.QuerySelectorAll("a");

		// Assert
		links.Should().HaveCount(5);
		links[0].GetAttribute("href").Should().Be("https://www.threads/");
		links[1].GetAttribute("href").Should().Be("https://www.instagram.com/");
		links[2].GetAttribute("href").Should().Be("https://www.youtube.com/");
		links[0].FirstElementChild!.ClassName.Should().Contain("ri-threads-line");
		links[1].FirstElementChild!.ClassName.Should().Contain("ri-instagram-line");
		links[2].FirstElementChild!.ClassName.Should().Contain("ri-youtube-line");

	}

	/// <summary>
	///   Validates that the RecentRelatedComponent displays the correct recent post-links and titles.
	/// </summary>
	[Fact]
	public void RecentRelatedComponent_Should_Render_Valid_Data()
	{

		// Arrange
		var cut = Render<ArticlesView>();

		// Act
		var recentSection = cut.Find("div.pl-6.pt-12.lg\\:pt-4");
		var postLinks = recentSection.QuerySelectorAll("div.flex.flex-col.gap-4 > div > a");

		// Assert
		postLinks.Should().HaveCount(2);
		postLinks[0].GetAttribute("href").Should().Be("/run-sql-server-on-m1-or-m2-macbook");
		postLinks[0].TextContent.Should().Contain("Run SQL Server on M1 or M2 Macbook");
		postLinks[1].GetAttribute("href").Should().Be("/run-sql-server-on-m1-or-m2-macbook");
		postLinks[1].TextContent.Should().Contain("Run a Postgres Database for Free on Google Cloud");

	}

}