// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryListTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Features.Categories.Components;

/// <summary>
///   Unit tests for <see cref="CategoryList" />
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(CategoryList))]
public class CategoryListTests : BunitContext
{

	private readonly ICategoryService _categoryServiceSub = Substitute.For<ICategoryService>();

	public CategoryListTests()
	{

		Services.AddSingleton(_categoryServiceSub);

	}

	[Fact]
	public void RendersLoadingSpinner_WhenIsLoading()
	{

		// Arrange
		var tcs = new TaskCompletionSource<Result<List<CategoryDto>>>();
		_categoryServiceSub.GetAllAsync().Returns(tcs.Task);
		var cut = Render<CategoryList>();

		// Act & Assert
		// While the service task is not completed, the component should be loading
		cut.Markup.Should().Contain("animate-spin");

		// Complete the service call to avoid test hang
		tcs.SetResult(Result.Ok(new List<CategoryDto>()));

	}

	[Fact]
	public void RendersNoCategories_WhenCategoriesIsNullOrEmptyAndResultIsOk()
	{

		// Arrange
		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(new List<CategoryDto>()));

		// Act
		var cut = Render<CategoryList>();

		// Assert
		cut.Markup.Should().Contain("Categories");
		cut.Markup.Should().Contain("bg-gray-50");

	}

	[Fact]
	public void RendersNoCategories_WhenCategoriesIsNullOrEmpty()
	{

		// Arrange
		const string expectedHtml =
				"""
				<div class="container mx-auto px-4 py-8">
					<div class="flex justify-between items-center mb-6">
						<header class="mx-auto max-w-7xl mb-6
										px-4 py-4 sm:px-4 md:px-6 lg:px-8 
										rounded-md shadow-md 
										shadow-blue-500">
							<h1 class="text-3xl font-bold tracking-tight text-gray-50 py-4">Categories</h1>
						</header>
						<a href="/categories/create" class="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition-colors">
							Create New
						</a>
					</div>
					<div class="bg-gray-50 rounded-lg p-8 text-center">
						<p class="text-lg text-gray-600">No categories found.</p>
						<p class="mt-2 text-gray-500">Create a new category to get started.</p>
					</div>
				</div>
				""";

		_categoryServiceSub.GetAllAsync().Returns(Result<List<CategoryDto>>.Fail("Failed to retrieve categories."));

		// Act
		var cut = Render<CategoryList>();

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

	[Fact]
	public void RendersCategories_WhenCategoriesArePresent()
	{

		// Arrange
		var categories = new List<CategoryDto>
		{
				new() { Id = ObjectId.GenerateNewId(), Name = "TestCat1", Description = "Desc1", CreatedOn = DateTime.Now },
				new() { Id = ObjectId.GenerateNewId(), Name = "TestCat2", Description = "Desc2", CreatedOn = DateTime.Now }
		};

		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(categories));
		var cut = Render<CategoryList>();

		// Assert
		cut.Markup.Should().Contain("TestCat1");
		cut.Markup.Should().Contain("TestCat2");

	}

	[Fact]
	public async Task ArchiveCategory_CallsService_WhenConfirmed()
	{

		// Arrange
		var category = new CategoryDto
				{ Id = ObjectId.GenerateNewId(), Name = "ToDelete", Description = "Desc", CreatedOn = DateTime.Now };

		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(new List<CategoryDto> { category }));
		_categoryServiceSub.ArchiveAsync(category).Returns(Result.Ok());
		var jsRuntime = Substitute.For<IJSRuntime>();
		jsRuntime.InvokeAsync<bool>("confirm", Arg.Any<object[]>()).Returns(_ => new ValueTask<bool>(true));
		Services.AddSingleton(jsRuntime);
		var cut = Render<CategoryList>();

		// Act
		await cut.InvokeAsync(() => cut.Instance.ArchiveCategory(category));

		// Assert
		await _categoryServiceSub.Received(1).ArchiveAsync(category);

	}

	[Fact]
	public async Task ArchiveCategory_DoesNotCallService_WhenNotConfirmed()
	{

		// Arrange
		var category = new CategoryDto
				{ Id = ObjectId.GenerateNewId(), Name = "ToDelete", Description = "Desc", CreatedOn = DateTime.Now };

		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(new List<CategoryDto> { category }));
		var jsRuntime = Substitute.For<IJSRuntime>();
		jsRuntime.InvokeAsync<bool>("confirm", Arg.Any<object[]>()).Returns(_ => new ValueTask<bool>(false));
		Services.AddSingleton(jsRuntime);
		var cut = Render<CategoryList>();

		// Act
		await cut.InvokeAsync(() => cut.Instance.ArchiveCategory(category));

		// Assert
		await _categoryServiceSub.DidNotReceive().ArchiveAsync(category);

	}

	[Fact]
	public async Task Refreshes_List_After_Successful_Archive()
	{

		// Arrange
		var categoriesDto = FakeCategoryDto.GetCategoriesDto(3, true);
		categoriesDto[0].Archived = false;
		
		var categoriesDto2 = categoriesDto.ToList();
		categoriesDto2[0].Archived = true; // Simulate archiving the first category
		
		_categoryServiceSub.GetAllAsync().Returns(
				Result.Ok(categoriesDto),
				Result.Ok(categoriesDto2) // Second call after archive
		);

		_categoryServiceSub.ArchiveAsync(categoriesDto[0]).Returns(Result.Ok());

		var jsRuntime = Substitute.For<IJSRuntime>();
		jsRuntime.InvokeAsync<bool>("confirm", Arg.Any<object[]>()).Returns(true);
		Services.AddSingleton(jsRuntime);

		var cut = Render<CategoryList>();

		// Act
		await cut.InvokeAsync(() => cut.Instance.ArchiveCategory(categoriesDto[0]));

		// Assert
		await _categoryServiceSub.Received(2).GetAllAsync(); // Verify list was refreshed
		cut.Markup.Should().Contain("""<span class="px-2 py-1 text-xs font-medium rounded-full bg-red-100 text-red-800">Archived</span>"""); // Verify UI updated

	}

	[Fact]
	public async Task Refreshes_List_After_Failed_Archive()
	{

		// Arrange
		const string expectedHtml =
				"""
				<div class="container mx-auto px-4 py-8">
					<div class="flex justify-between items-center mb-6">
						<header class="mx-auto max-w-7xl mb-6
								px-4 py-4 sm:px-4 md:px-6 lg:px-8 
								rounded-md shadow-md 
								shadow-blue-500">
				      <h1 class="text-3xl font-bold tracking-tight text-gray-50 py-4">Categories</h1>
				    </header>
				    <a href="/categories/create" class="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition-colors">Create 			New</a>
				  </div>
				  <div class="bg-white shadow-md rounded-lg overflow-hidden">
				    <table class="min-w-full divide-y divide-gray-200">
				      <thead class="bg-gray-50">
				        <tr>
				          <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Name
				          </th>
				          <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
				            Description
				          </th>
				          <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Created 							On
				          </th>
				          <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
				            Status
				          </th>
				          <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
				            Actions
				          </th>
				        </tr>
				      </thead>
				      <tbody class="bg-white divide-y divide-gray-200">
				        <tr>
				          <td class="px-6 py-4 whitespace-nowrap">
				            <a href:ignore class="text-blue-600 hover:underline">Rustic Steel Pizza</a>
				          </td>
				          <td class="px-6 py-4">rustic_steel_pizza</td>
				          <td class="px-6 py-4 whitespace-nowrap">1/1/0001</td>
				          <td class="px-6 py-4 whitespace-nowrap">
				            <span class="px-2 py-1 text-xs font-medium rounded-full bg-green-100 text-green-800">Active</span>
				          </td>
				          <td class="px-6 py-4 whitespace-nowrap">
				            <div class="flex space-x-3">
				              <a href:ignore class="text-blue-600 hover:text-blue-900">View</a>
				              <a href:ignore class="text-indigo-600 hover:text-indigo-900">Edit</a>
				              <button class="text-red-600 hover:text-red-900" >
				                Archive
				              </button>
				            </div>
				          </td>
				        </tr>
				        <tr>
				          <td class="px-6 py-4 whitespace-nowrap">
				            <a href:ignore class="text-blue-600 hover:underline">Ergonomic Rubber Bike</a>
				          </td>
				          <td class="px-6 py-4">ergonomic_rubber_bike</td>
				          <td class="px-6 py-4 whitespace-nowrap">1/1/0001</td>
				          <td class="px-6 py-4 whitespace-nowrap">
				            <span class="px-2 py-1 text-xs font-medium rounded-full bg-green-100 text-green-800">Active</span>
				          </td>
				          <td class="px-6 py-4 whitespace-nowrap">
				            <div class="flex space-x-3">
				              <a href:ignore class="text-blue-600 hover:text-blue-900">View</a>
				              <a href:ignore class="text-indigo-600 hover:text-indigo-900">Edit</a>
				              <button class="text-red-600 hover:text-red-900" >
				                Archive
				              </button>
				            </div>
				          </td>
				        </tr>
				        <tr>
				          <td class="px-6 py-4 whitespace-nowrap">
				            <a href:ignore class="text-blue-600 hover:underline">Rustic Frozen Bacon</a>
				          </td>
				          <td class="px-6 py-4">rustic_frozen_bacon</td>
				          <td class="px-6 py-4 whitespace-nowrap">1/1/0001</td>
				          <td class="px-6 py-4 whitespace-nowrap">
				            <span class="px-2 py-1 text-xs font-medium rounded-full bg-green-100 text-green-800">Active</span>
				          </td>
				          <td class="px-6 py-4 whitespace-nowrap">
				            <div class="flex space-x-3">
				              <a href:ignore class="text-blue-600 hover:text-blue-900">View</a>
				              <a href:ignore class="text-indigo-600 hover:text-indigo-900">Edit</a>
				              <button class="text-red-600 hover:text-red-900" >
				                Archive
				              </button>
				            </div>
				          </td>
				        </tr>
				      </tbody>
				    </table>
				  </div>
				</div>
				""";

		var categoriesDto = FakeCategoryDto.GetCategoriesDto(3, true);

		_categoryServiceSub.GetAllAsync().Returns(
				Result.Ok(categoriesDto)
		);

		_categoryServiceSub.ArchiveAsync(categoriesDto[0]).Returns(Result.Fail("Failed to archive category"));

		var jsRuntime = Substitute.For<IJSRuntime>();
		jsRuntime.InvokeAsync<bool>("confirm", Arg.Any<object[]>()).Returns(true);
		Services.AddSingleton(jsRuntime);

		var cut = Render<CategoryList>();

		// Act
		await cut.InvokeAsync(() => cut.Instance.ArchiveCategory(categoriesDto[0]));

		// Assert
		await _categoryServiceSub.Received(1).GetAllAsync(); // Verify list was refreshed
		cut.MarkupMatches(expectedHtml); // Verify UI updated

	}

	[Fact]
	public void Truncates_Long_Descriptions_In_List()
	{

		// Arrange
		var longDesc = new string('x', 100);

		var categories = new List<CategoryDto>
		{
				new()
				{
						Id = ObjectId.GenerateNewId(),
						Name = "Test",
						Description = longDesc,
						CreatedOn = DateTime.Now
				}
		};

		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(categories));

		// Act
		var cut = Render<CategoryList>();

		// Assert
		cut.Markup.Should().NotContain(longDesc);
		cut.Markup.Should().Contain("...");

	}

}