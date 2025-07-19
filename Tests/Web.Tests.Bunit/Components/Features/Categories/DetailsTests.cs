// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DetailsTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Features.Categories;

/// <summary>
///   Unit tests for <see cref="Details" />
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(Details))]
public class DetailsTests : BunitContext
{

	private readonly ICategoryService _categoryServiceSub = Substitute.For<ICategoryService>();

	public DetailsTests()
	{

		Services.AddSingleton(_categoryServiceSub);

	}

	[Fact]
	public void RendersLoadingSpinner_WhenIsLoading()
	{

		// Arrange
		var tcs = new TaskCompletionSource<Result<CategoryDto>>();
		_categoryServiceSub.GetAsync(Arg.Any<ObjectId>()).Returns(_ => tcs.Task);
		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, ObjectId.GenerateNewId()));

		// Act & Assert
		// While the service task is not completed, the component should be loading
		cut.Markup.Should().Contain("animate-spin");

		// Complete the service call to avoid test hang
		tcs.SetResult(Result.Ok(CategoryDto.Empty));

	}

	[Fact]
	public void RendersNotFound_WhenCategoryIsNull()
	{

		// Arrange
		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, ObjectId.GenerateNewId()));

		// Act
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, null);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Categories not found");
		cut.Markup.Should().Contain("Return to categories");

	}
	[Fact]
	public void RendersCategoryDetails_WhenCategoryIsPresent()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete and category loaded
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain(categoryDto.Name);
	}

	[Fact]
	public async Task CallsCategoryService_OnInitializedAsync()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));


		// Assert
		await _categoryServiceSub.Received(1).GetAsync(Arg.Is<ObjectId>(id => id == categoryDto.Id));

		cut.Instance.GetType().GetProperty("_category")?.GetValue(cut.Instance)
				.Should().Be(categoryDto);

	}

	[Fact]
	public void DisplaysNever_WhenModifiedOnIsNull()
	{
		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		categoryDto.ModifiedOn = null;
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Never");
	}

	[Fact]
	public void DisplaysModifiedOn_WhenPresent()
	{
		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		categoryDto.ModifiedOn = DateTime.UtcNow;
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain(categoryDto.ModifiedOn.Value.ToLocalTime().ToString("g"));
	}

	[Fact]
	public void DisplaysCorrectStatus_WhenActive()
	{
		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		categoryDto.Archived = false;
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Active");
		cut.Find(".bg-green-100").Should().NotBeNull();
	}

	[Fact]
	public void DisplaysCorrectStatus_WhenArchived()
	{
		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		categoryDto.Archived = true;
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Archived");
		cut.Find(".bg-red-100").Should().NotBeNull();
	}

	[Fact]
	public void HasCorrectNavigationLinks()
	{
		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Assert
		cut.Find("a[href='/categories']").Should().NotBeNull();
		cut.Find($"a[href='/categories/edit/{categoryDto.Id}']").Should().NotBeNull();
	}

	[Fact]
	public void HandlesServiceException_Gracefully()
	{
		// Arrange
		var categoryId = ObjectId.GenerateNewId(); _categoryServiceSub.GetAsync(categoryId).Returns(Task.FromException<Result<CategoryDto>>(new Exception("Service error")));

		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, categoryId));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Categories not found or has been deleted");
	}

	[Fact]
	public void HandlesEmptyObjectId()
	{
		// Arrange
		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, ObjectId.Empty));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Categories not found or has been deleted");
	}

	[Fact]
	public void FormatsDates_InLocalTimeZone()
	{
		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		categoryDto.CreatedOn = DateTime.UtcNow.AddDays(-1);
		categoryDto.ModifiedOn = DateTime.UtcNow;
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain(categoryDto.CreatedOn.ToLocalTime().ToString("g"));
		cut.Markup.Should().Contain(categoryDto.ModifiedOn.Value.ToLocalTime().ToString("g"));
	}

	[Fact]
	public void Shows_Metadata_Section_WhenDisplayingCategory()
	{
		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Details>(parameters => parameters
			.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Assert
		cut.FindAll("dt").Count.Should().BeGreaterThan(0); // Has metadata labels
		cut.FindAll("dd").Count.Should().BeGreaterThan(0); // Has metadata values
	}

}