// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ListTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Features.Categories;

/// <summary>
///   Unit tests for <see cref="List{T}" />
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(List))]
public class ListTests : BunitContext
{

	private readonly ICategoryService _categoryServiceSub = Substitute.For<ICategoryService>();

	public ListTests()
	{

		Services.AddScoped<CascadingAuthenticationState>();
		Services.AddSingleton(_categoryServiceSub);

	}

	[Fact]
	public void RendersNoCategories_WhenCategoriesIsNullOrEmptyAndResultIsOk()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(new List<CategoryDto>()));

		// Act
		var cut = Render<List>();

		// Assert
		cut.Markup.Should().Contain("No categories available.");
		cut.Markup.Should().Contain("Create New Category");

	}

	[Fact]
	public void RendersNoCategories_WhenCategoriesIsNullOrEmpty()
	{

		// Arrange
		Helpers.SetAuthorization(this);

		const string expectedHtml =
				"""
				<header class="mx-auto 
					max-w-7xl 
					mb-6
					p-1 
					sm:px-4 
					md:px-6 
					lg:px-8 
					rounded-md 
					shadow-md 
					shadow-blue-500">
				  <h1 class="text-3xl font-bold tracking-tight blue-500">Categories</h1>
				</header>
				<div class="alert alert-info">
				  No categories available.
				  <button class="btn btn-success mt-3" >Create New Category</button>
				</div>
				""";

		_categoryServiceSub.GetAllAsync().Returns(Result<List<CategoryDto>>.Fail("Failed to retrieve categories."));

		// Act
		var cut = Render<List>();

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

	[Fact]
	public void RendersCategories_WhenCategoriesArePresent()
	{
		// Arrange
		JSInterop.Mode = JSRuntimeMode.Loose;

		//JSInterop.SetupModule("./_content/Microsoft.AspNetCore.Components.QuickGrid/QuickGrid.razor.js");
		Helpers.SetAuthorization(this);
		var categoriesDto = FakeCategoryDto.GetCategoriesDto(2, true);

		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(categoriesDto));

		// Act
		var cut = Render<List>();

		// Assert
		cut.WaitForAssertion(() =>
		{
			cut.Markup.Should().Contain(categoriesDto[0].Name);
			cut.Markup.Should().Contain(categoriesDto[1].Name);
		});
	}

}