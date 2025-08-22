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
		Services.AddCascadingAuthenticationState();

	}

	[Fact]
	public void RendersNotFound_WhenCategoryIsNull()
	{

		// Arrange
		Helpers.SetAuthorization(this);

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, Guid.CreateVersion7()));

		// Act
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, null);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Category not found");

	}

	[Fact]
	public void RendersCategoryDetails_WhenCategoryIsPresent()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete and article loaded
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain(categoryDto.Name);
		cut.Markup.Should().Contain("Created On: 1/1/2025");
		cut.Markup.Should().Contain("Modified On: 1/1/2025");
		cut.Find("button.btn-secondary").Should().NotBeNull();
		cut.Find("button.btn-light").Should().NotBeNull();

	}

	[Fact]
	public void HasCorrectNavigationButtons()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Assert
		cut.Find("button.btn-secondary").Should().NotBeNull();
		cut.Find("button.btn-light").Should().NotBeNull();

	}

	[Fact]
	public void NavigatesToEditPage_WhenEditButtonClicked()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var navigationManager = Services.GetRequiredService<BunitNavigationManager>();

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Act
		cut.Find("button.btn-secondary").Click();

		// Assert
		navigationManager.Uri.Should().EndWith($"/categories/edit/{categoryDto.Id}");

	}

	[Fact]
	public void NavigatesToListPage_WhenBackButtonClicked()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var navigationManager = Services.GetRequiredService<BunitNavigationManager>();

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_category")?.SetValue(cut.Instance, categoryDto);
		cut.Render();

		// Act
		cut.Find("button.btn-light").Click();

		// Assert
		navigationManager.Uri.Should().EndWith("/categories");
	}

	[Fact]
	public void HandlesEmptyGuid()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		_categoryServiceSub.GetAsync(Guid.Empty).Returns(Result.Fail<CategoryDto>("Not found"));

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, Guid.Empty));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Category not found");

	}

	[Fact]
	public void HandlesServiceException_Gracefully()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var articleId = Guid.CreateVersion7();

		_categoryServiceSub.GetAsync(articleId)
				.Returns(Task.FromException<Result<CategoryDto>>(new Exception("Service error")));

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, articleId));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Category not found");

	}

	[Fact]
	public async Task CallsCategoryService_OnInitializedAsync()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Assert
		await _categoryServiceSub.Received(1).GetAsync(Arg.Is<Guid>(id => id == categoryDto.Id));

	}

}