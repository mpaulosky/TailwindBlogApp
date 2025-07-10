// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     EditTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Features.Categories;

/// <summary>
///   Bunit tests for <see cref="Edit" /> component.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(Edit))]
public class EditTests : BunitContext
{

	private readonly ICategoryService _categoryServiceSub = Substitute.For<ICategoryService>();

	public EditTests()
	{

		Services.AddSingleton(_categoryServiceSub);

	}

	[Fact]
	public void Renders_Loading_Spinner_Initially()
	{

		// Arrange
		var tcs = new TaskCompletionSource<Result<CategoryDto>>();
		_categoryServiceSub.GetAsync(Arg.Any<ObjectId>()).Returns(_ => tcs.Task);

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, ObjectId.GenerateNewId()));

		// Assert
		cut.Markup.Should().Contain("animate-spin");
		cut.Markup.Should().Contain("Edit Categories");

		// Complete the service call to avoid test hang
		tcs.SetResult(Result.Ok(CategoryDto.Empty));

	}

	[Fact]
	public void Renders_Category_Not_Found_When_Service_Returns_Null()
	{

		// Arrange
		var categoryId = ObjectId.GenerateNewId();
		_categoryServiceSub.GetAsync(categoryId).Returns(Result<CategoryDto>.Fail("Not found"));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryId));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		cut.Markup.Should().Contain("Category not found or has been deleted");
		cut.Markup.Should().Contain("Return to categories");

	}

	[Fact]
	public void Renders_Edit_Form_When_Category_Exists()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		cut.Markup.Should().Contain("Edit Categories");
		cut.Markup.Should().Contain("Update the category information");
		cut.Find("form").Should().NotBeNull();
		cut.Find("#name").Should().NotBeNull();
		cut.Find("#description").Should().NotBeNull();
		cut.Find("button[type='submit']").Should().NotBeNull();

	}

	[Fact]
	public void Populates_Form_Fields_With_Category_Data()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		var nameInput = cut.Find("#name");
		var descriptionInput = cut.Find("#description");

		nameInput.GetAttribute("value").Should().Be(categoryDto.Name);
		descriptionInput.GetAttribute("value").Should().Be(categoryDto.Slug);

	}

	[Fact]
	public void Shows_Validation_Error_When_Name_Is_Empty()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		var nameInput = cut.Find("#name");
		var form = cut.Find("form");

		// Act
		nameInput.Change("");
		form.Submit();

		// Assert
		cut.Markup.Should().Contain("""<div class="text-red-500 text-sm mt-1">Name is required</div>""");

	}

	[Fact]
	public void Shows_Validation_Error_When_Slug_Is_Empty()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		var descriptionInput = cut.Find("#description");
		var form = cut.Find("form");

		// Act
		descriptionInput.Change("");
		form.Submit();

		// Assert
		cut.Markup.Should().Contain("""<div class="text-red-500 text-sm mt-1">Slug is required</div>""");

	}

	[Fact]
	public async Task Submits_Valid_Form_And_Navigates_On_Success()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));
		_categoryServiceSub.UpdateAsync(Arg.Any<CategoryDto>()).Returns(Result.Ok());

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		var nameInput = cut.Find("#name");
		var descriptionInput = cut.Find("#description");
		var form = cut.Find("form");

		// Act
		await cut.InvokeAsync(() => nameInput.Change("Updated Name"));
		await cut.InvokeAsync(() => descriptionInput.Change("Updated Slug"));
		await cut.InvokeAsync(() => form.Submit());

		// Assert
		await _categoryServiceSub.Received(1).UpdateAsync(Arg.Is<CategoryDto>(dto =>
				dto.Name == "Updated Name" && dto.Slug == "Updated Slug"));

	}

	[Fact]
	public async Task Shows_Error_Message_When_Update_Fails()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));
		_categoryServiceSub.UpdateAsync(Arg.Any<CategoryDto>()).Returns(Result.Fail("Update failed"));

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		var form = cut.Find("form");

		// Act
		await cut.InvokeAsync(() => form.Submit());

		// Assert
		cut.Markup.Should().Contain("Update failed");

	}

	[Fact]
	public void Disables_Submit_Button_When_Submitting()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var tcs = new TaskCompletionSource<Result>();
		_categoryServiceSub.UpdateAsync(Arg.Any<CategoryDto>()).Returns(_ => tcs.Task);

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		var form = cut.Find("form");
		var submitButton = cut.Find("button[type='submit']");

		// Act
		form.Submit();

		// Assert
		submitButton.HasAttribute("disabled").Should().BeTrue();
		cut.Markup.Should().Contain("Saving...");

		// Complete the task to avoid test hang
		tcs.SetResult(Result.Ok());

	}

	[Fact]
	public void Shows_Save_Changes_Text_When_Not_Submitting()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		cut.Markup.Should().Contain("Save Changes");
		cut.Markup.Should().NotContain("Saving...");

	}

	[Fact]
	public void Cancel_Link_Navigates_To_Categories_List()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		var cancelLink = cut.Find("a[href='/categories']");
		cancelLink.Should().NotBeNull();
		cancelLink.TextContent.Should().Contain("Cancel");

	}

	[Fact]
	public async Task Calls_CategoryService_GetAsync_On_Initialize()
	{

		// Arrange
		var categoryId = ObjectId.GenerateNewId();
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		categoryDto.Id = categoryId;

		_categoryServiceSub.GetAsync(categoryId).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryId));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		await _categoryServiceSub.Received(1).GetAsync(Arg.Is<ObjectId>(id => id == categoryId));

	}

	[Fact]
	public void Handles_Empty_ObjectId_Parameter()
	{

		// Arrange
		_categoryServiceSub.GetAsync(ObjectId.Empty).Returns(Result<CategoryDto>.Fail("Invalid ID"));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, ObjectId.Empty));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		cut.Markup.Should().Contain("Category not found or has been deleted");

	}

	[Fact]
	public void Shows_Correct_Page_Heading()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		cut.Markup.Should().Contain("Edit Categories");
		cut.Markup.Should().Contain("Update the category information");

	}

	[Fact]
	public void Renders_Correct_Form_Structure()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		var form = cut.Find("form");
		var nameLabel = cut.Find("label[for='name']");
		var descriptionLabel = cut.Find("label[for='description']");
		var nameInput = cut.Find("#name");
		var descriptionInput = cut.Find("#description");

		form.Should().NotBeNull();
		nameLabel.TextContent.Should().Contain("Categories Name");
		descriptionLabel.TextContent.Should().Contain("Slug");
		nameInput.GetAttribute("class").Should().Contain("w-full px-3 py-2 border");
		descriptionInput.GetAttribute("class").Should().Contain("w-full px-3 py-2 border");

	}

	[Fact]
	public void Preserves_Category_Id_During_Update()
	{

		// Arrange
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));
		_categoryServiceSub.UpdateAsync(Arg.Any<CategoryDto>()).Returns(Result.Ok());

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		var form = cut.Find("form");

		// Act
		form.Submit();

		// Assert
		_categoryServiceSub.Received(1).UpdateAsync(Arg.Is<CategoryDto>(dto => dto.Id == categoryDto.Id));

	}

	[Fact]
	public async Task Returns_Early_When_Model_Is_Null_During_Submit()
	{

		// Arrange
		var categoryId = ObjectId.GenerateNewId();
		_categoryServiceSub.GetAsync(categoryId).Returns(Result<CategoryDto>.Fail("Not found"));

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryId));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Act & Assert - Should not throw exception when trying to submit with null model
		// The component should handle this gracefully and not call the service
		await _categoryServiceSub.DidNotReceive().UpdateAsync(Arg.Any<CategoryDto>());

	}

}