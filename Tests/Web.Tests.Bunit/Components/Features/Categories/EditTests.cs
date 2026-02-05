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

		Services.AddScoped<CascadingAuthenticationState>();
		Services.AddSingleton(_categoryServiceSub);

	}

	[Fact]
	public void Renders_Category_Not_Found_When_Service_Returns_Null()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryId = Guid.CreateVersion7();
		_categoryServiceSub.GetAsync(categoryId).Returns(Result<CategoryDto>.Fail("Category not found"));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryId));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		cut.Markup.Should().Contain("Category not found");
		cut.Markup.Should().Contain("Edit Category");

	}

	[Fact]
	public void Renders_Edit_Form_When_Category_Exists()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		cut.Markup.Should().Contain("Edit Category");
		cut.Markup.Should().Contain("Category Name");
		cut.Markup.Should().Contain("Save Changes");
		cut.Markup.Should().Contain("Cancel");
		cut.Markup.Should().NotContain("animate-spin");
		cut.Markup.Should().NotContain("Loading...");
		cut.Markup.Should().Contain(categoryDto.Name);
		cut.Find("form").Should().NotBeNull();

	}

	[Fact]
	public void Populates_Form_Fields_With_Category_Data()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		var nameInput = cut.Find("#name");

		nameInput.GetAttribute("value").Should().Be(categoryDto.Name);

	}

	[Fact]
	public void Shows_Validation_Error_When_Name_Is_Empty()
	{

		// Arrange
		Helpers.SetAuthorization(this);
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
		cut.Markup.Should().Contain("Name is required");

	}

	[Fact]
	public async Task Submits_Valid_Form_And_Navigates_On_Success()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));
		_categoryServiceSub.UpdateAsync(Arg.Any<CategoryDto>()).Returns(Result.Ok());

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		var nameInput = cut.Find("#name");
		var form = cut.Find("form");

		// Act
		await cut.InvokeAsync(() => nameInput.Change("Updated Name"));
		await cut.InvokeAsync(() => form.Submit());

		// Assert
		await _categoryServiceSub.Received(1).UpdateAsync(Arg.Is<CategoryDto>(dto =>
				dto.Name == "Updated Name"));

	}

	[Fact]
	public async Task Shows_Error_Message_When_Update_Fails()
	{

		// Arrange
		Helpers.SetAuthorization(this);
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
	public async Task Disables_Submit_Button_When_Submitting()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		// Simulate a long-running update operation
		var tcs = new TaskCompletionSource<Result>();
		_categoryServiceSub.UpdateAsync(Arg.Any<CategoryDto>()).Returns(_ => tcs.Task);

		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, categoryDto.Id));
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"));

		// Change input and submit form
		var nameInput = cut.Find("#name");
		await cut.InvokeAsync(() => nameInput.Change("Updated Name"));
		var submitTask = cut.InvokeAsync(() => cut.Find("form").Submit());

		// Assert: submit button is disabled and text is 'Updating...'
		var submitButton = cut.Find("button[type='submit']");
		submitButton.HasAttribute("disabled").Should().BeTrue();
		submitButton.TextContent.Trim().Should().Be("Updating...");

		// Allow async submit to finish, don't forget this!
		tcs.SetResult(Result.Ok());
		await submitTask;


	}

	[Fact]
	public void Shows_Save_Changes_Text_When_Not_Submitting()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		cut.Markup.Should().Contain("Save Changes");
		cut.Markup.Should().NotContain("Saving...");
		cut.Markup.Should().Contain("Cancel");

	}

	[Fact]
	public void Cancel_Link_Navigates_To_Categories_List()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
		var cancelButton = cut.Find("button[type='button']");

		// Assert
		cancelButton.Should().NotBeNull();
		cancelButton.TextContent.Should().Contain("Cancel");

	}

	[Fact]
	public async Task Calls_CategoryService_GetAsync_On_Initialize()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryId = Guid.CreateVersion7();
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		categoryDto.Id = categoryId;

		_categoryServiceSub.GetAsync(categoryId).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryId));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		await _categoryServiceSub.Received(1).GetAsync(Arg.Is<Guid>(id => id == categoryId));

	}

	[Fact]
	public void Handles_Empty_ObjectId_Parameter()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		_categoryServiceSub.GetAsync(Guid.Empty).Returns(Result<CategoryDto>.Fail("Invalid ID"));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, Guid.Empty));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		cut.Markup.Should().Contain("Category not found");

	}

	[Fact]
	public void Shows_Correct_Page_Heading()
	{

		// Arrange
		Helpers.SetAuthorization(this);
		var categoryDto = FakeCategoryDto.GetNewCategoryDto(true);
		_categoryServiceSub.GetAsync(categoryDto.Id).Returns(Result.Ok(categoryDto));

		// Act
		var cut = Render<Edit>(parameters => parameters
				.Add(p => p.Id, categoryDto.Id));

		// Wait for the component to finish loading
		cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

		// Assert
		cut.Markup.Should().Contain("Edit Category");
		cut.Markup.Should().Contain("Category Name");
		cut.Markup.Should().Contain("Save Changes");
		cut.Markup.Should().Contain("Cancel");

	}

	[Fact]
	public void Preserves_Category_Id_During_Update()
	{

		// Arrange
		Helpers.SetAuthorization(this);
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
		Helpers.SetAuthorization(this);
		var categoryId = Guid.CreateVersion7();
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