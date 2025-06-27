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
		var categoriesDto = FakeCategoryDto.GetCategoriesDto(2, true);

		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(categoriesDto));

		// Act
		var cut = Render<CategoryList>();

		// Assert
		cut.Markup.Should().Contain(categoriesDto[0].Name);
		cut.Markup.Should().Contain(categoriesDto[1].Name);

	}

	[Fact]
	public async Task ArchiveCategory_CallsService_WhenConfirmed()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);

		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(new List<CategoryDto> { categoryDto }));

		_categoryServiceSub.ArchiveAsync(categoryDto).Returns(Result.Ok());

		var jsRuntime = Substitute.For<IJSRuntime>();

		jsRuntime.InvokeAsync<bool>("confirm", Arg.Any<object[]>()).Returns(_ => new ValueTask<bool>(true));

		Services.AddSingleton(jsRuntime);

		var cut = Render<CategoryList>();

		// Act
		await cut.InvokeAsync(() => cut.Instance.ArchiveCategory(categoryDto));

		// Assert
		await _categoryServiceSub.Received(1).ArchiveAsync(categoryDto);

	}

	[Fact]
	public async Task ArchiveCategory_DoesNotCallService_WhenNotConfirmed()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);

		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(new List<CategoryDto> { categoryDto }));

		var jsRuntime = Substitute.For<IJSRuntime>();

		jsRuntime.InvokeAsync<bool>("confirm", Arg.Any<object[]>()).Returns(_ => new ValueTask<bool>(false));

		Services.AddSingleton(jsRuntime);
		
		var cut = Render<CategoryList>();

		// Act
		await cut.InvokeAsync(() => cut.Instance.ArchiveCategory(categoryDto));

		// Assert
		await _categoryServiceSub.DidNotReceive().ArchiveAsync(categoryDto);

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

		cut.Markup.Should()
				.Contain(
						"""<span class="px-2 py-1 text-xs font-medium rounded-full bg-red-100 text-red-800">Archived</span>"""); // Verify UI updated

	}

	[Fact]
	public async Task Refreshes_List_After_Failed_Archive()
	{

		// Arrange
		var categoriesDto = FakeCategoryDto.GetCategoriesDto(1, true);

		_categoryServiceSub.GetAllAsync().Returns(Result.Ok(categoriesDto));

		_categoryServiceSub.ArchiveAsync(categoriesDto[0]).Returns(Result.Fail("Failed to archive category"));

		var jsRuntime = Substitute.For<IJSRuntime>();
		jsRuntime.InvokeAsync<bool>("confirm", Arg.Any<object[]>()).Returns(true);
		Services.AddSingleton(jsRuntime);

		var cut = Render<CategoryList>();

		// Act
		await cut.InvokeAsync(() => cut.Instance.ArchiveCategory(categoriesDto[0]));

		// Assert
		await _categoryServiceSub.Received(1).GetAllAsync(); // Verify list was refreshed
		cut.Markup.Should().Contain("Archive");

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

	[Fact]
	public async Task Shows_Archived_Status_And_Updates_ModifiedOn_After_Archive()
	{

		// Arrange
		var originalCreatedOn = DateTime.UtcNow.AddDays(-2);
		var originalModifiedOn = null as DateTime?;
		var archivedModifiedOn = DateTime.UtcNow.AddDays(-1);

		var categoryDto = new CategoryDto
		{
			Id = ObjectId.GenerateNewId(),
			Name = "ToArchive",
			Description = "Desc",
			CreatedOn = originalCreatedOn,
			ModifiedOn = originalModifiedOn,
			Archived = false
		};

		var archivedCategoryDto = new CategoryDto
		{
			Id = categoryDto.Id,
			Name = categoryDto.Name,
			Description = categoryDto.Description,
			CreatedOn = originalCreatedOn,
			ModifiedOn = archivedModifiedOn,
			Archived = true
		};

		_categoryServiceSub.GetAllAsync().Returns(
			Result.Ok(new List<CategoryDto> { categoryDto }),
			Result.Ok(new List<CategoryDto> { archivedCategoryDto })
		);

		_categoryServiceSub.ArchiveAsync(categoryDto).Returns(Result.Ok());

		var jsRuntime = Substitute.For<IJSRuntime>();
		jsRuntime.InvokeAsync<bool>("confirm", Arg.Any<object[]>()).Returns(true);
		Services.AddSingleton(jsRuntime);

		var cut = Render<CategoryList>();

		// Act
		await cut.InvokeAsync(() => cut.Instance.ArchiveCategory(categoryDto));

		// Assert
		cut.Markup.Should().Contain("Archived");
		cut.Markup.Should().Contain(originalCreatedOn.ToString("M/d/yyyy"));
		cut.Markup.Should().Contain(archivedModifiedOn.ToString("M/d/yyyy"));

	}

}