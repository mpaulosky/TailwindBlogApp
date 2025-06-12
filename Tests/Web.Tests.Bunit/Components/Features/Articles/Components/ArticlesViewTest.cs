// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticlesViewTest.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Features.Articles.Components;

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
				      <article class="md:grid md:grid-cols-1 md:items-baseline">
				        <div class="relative isolate flex flex-col items-start rounded-2xl px-4 py-6 hover:bg-blue-500/10">
				          <h2 class="text-base font-semibold tracking-tight text-gray-50">
				            <a diff:ignore href="the-empathy-of-referential-interpretation">
				              <span class="absolute inset-0 z-10"></span>
				              The Empathy Of Referential Interpretation</a>
				          </h2>
				          <p diff:ignore class="mt-2 text-sm text-gray-50 py-2">The Attitude Of Consensus Vivacity</p>
				          <div class="flex gap-4 border-t border-gray-200 pt-4">
				            <div diff:ignore>Author: Mara41</div>
				            <div diff:ignore>Created: 1/1/2025</div>
				            <div diff:ignore>Draft</div>
				            <div diff:ignore>Category: Blog</div>
				          </div>
				        </div>
				      </article>
				    </div>
				    <div class="rounded-2xl shadow-md hover:shadow-blue-500">
				      <article class="md:grid md:grid-cols-1 md:items-baseline">
				        <div class="relative isolate flex flex-col items-start rounded-2xl px-4 py-6 hover:bg-blue-500/10">
				          <h2 class="text-base font-semibold tracking-tight text-gray-50">
				            <a diff:ignore href="the-problem-of-inductive-romanticism">
				              <span class="absolute inset-0 z-10"></span>
				              The Problem Of Inductive Romanticism</a>
				          </h2>
				          <p diff:ignore class="mt-2 text-sm text-gray-50 py-2">The Object Of Political Consciousness</p>
				          <div class="flex gap-4 border-t border-gray-200 pt-4">
				            <div diff:ignore>Author: Josephine4</div>
				            <div diff:ignore>Created: 1/1/2025</div>
				            <div diff:ignore>Published: 1/1/2025</div>
				            <div diff:ignore>Category: Blog</div>
				          </div>
				        </div>
				      </article>
				    </div>
				    <div class="rounded-2xl shadow-md hover:shadow-blue-500">
				      <article class="md:grid md:grid-cols-1 md:items-baseline">
				        <div class="relative isolate flex flex-col items-start rounded-2xl px-4 py-6 hover:bg-blue-500/10">
				          <h2 class="text-base font-semibold tracking-tight text-gray-50">
				            <a diff:ignore href="the-impulse-of-empirical-affectability">
				              <span class="absolute inset-0 z-10"></span>
				              The Impulse Of Empirical Affectability</a>
				          </h2>
				          <p diff:ignore class="mt-2 text-sm text-gray-50 py-2">The Determinism Of Integrated Principle</p>
				          <div class="flex gap-4 border-t border-gray-200 pt-4">
				            <div diff:ignore>Author: Erin14</div>
				            <div diff:ignore>Created: 1/1/2025</div>
				            <div diff:ignore>Published: 1/1/2025</div>
				            <div diff:ignore>Category: Blog</div>
				          </div>
				        </div>
				      </article>
				    </div>
				    }
				  </div>
				  <div class="pl-6 pt-12 lg:pt-4">
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

			// cut.Markup.Should().Contain("font-bold text-xl mb-2 test-blue-800");
			// cut.Markup.Should().Contain("Connect with us!");
			// cut.Markup.Should().Contain("Recent Posts");
		}

	}
