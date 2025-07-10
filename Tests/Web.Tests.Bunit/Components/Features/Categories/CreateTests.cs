// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CreateTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Features.Categories;

/// <summary>
///   Bunit tests for <see cref="Create" /> component.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(Create))]
public class CreateTests : BunitContext
{

	private readonly ICategoryService _categoryServiceSub = Substitute.For<ICategoryService>();

	public CreateTests()
	{

		Services.AddSingleton(_categoryServiceSub);

	}

	[Fact]
	public void Renders_Form_And_Heading()
	{

		// Arrange & Act
		var cut = Render<Create>();

		// Assert
		cut.Markup.Should().Contain("Create Categories");
		cut.Find("form").Should().NotBeNull();
		cut.Find("#name").Should().NotBeNull();

	}

	[Fact]
	public void Shows_Validation_Error_When_Name_Is_Empty()
	{

		// Arrange
		var cut = Render<Create>();
		var form = cut.Find("form");

		// Act
		form.Submit();

		// Assert
		cut.Markup.Should().Contain("""<div class="text-red-500 text-sm mt-1">Name is required</div>""");

	}

	[Fact(Skip = "Skipping due to incomplete test case")]
	public void Shows_Validation_Error_When_Description_Is_Empty()
	{
		// Arrange
		var cut = Render<Create>();
		var nameInput = cut.Find("#name");
		var form = cut.Find("form");

		// Act
		nameInput.Change("Test Name"); // Set valid name
		form.Submit();

		// Assert
		cut.Markup.Should().Contain("""<div class="text-red-500 text-sm mt-1">Description is required</div>""");

	}

	[Fact(Skip = "Skipping due to incomplete test case")]
	public async Task Submits_Valid_Form_And_Navigates()
	{

		// Arrange
		var cut = Render<Create>();
		var nameInput = cut.Find("#name");
		var descriptionInput = cut.Find("#description");
		var form = cut.Find("form");

		_categoryServiceSub.CreateAsync(Arg.Any<CategoryDto>())
				.Returns(Task.FromResult(Result.Ok()));

		// Act
		await cut.InvokeAsync(() => nameInput.Change("Test"));
		await cut.InvokeAsync(() => descriptionInput.Change("Test Description"));
		await cut.InvokeAsync(() => form.Submit());

		// Assert
		await _categoryServiceSub.Received(1).CreateAsync(Arg.Any<CategoryDto>());

	}

	[Fact(Skip = "Skipping due to incomplete test case")]
	public async Task Shows_Error_Message_On_Service_Exception()
	{

		// Arrange
		var cut = Render<Create>();
		var nameInput = cut.Find("#name");
		var descriptionInput = cut.Find("#description");
		var form = cut.Find("form");

		_categoryServiceSub.CreateAsync(Arg.Any<CategoryDto>())
				.Returns(Task.FromResult(Result.Fail("Service error")));

		// Act
		await cut.InvokeAsync(() => nameInput.Change("Test"));
		await cut.InvokeAsync(() => descriptionInput.Change("Test Description"));
		await cut.InvokeAsync(() => form.Submit());

		// Assert
		cut.Markup.Should().Contain("Service error");

	}

	[Fact(Skip = "Skipping due to incomplete test case")]
	public void Disables_Submit_Button_While_Submitting()
	{
		// Arrange
		var cut = Render<Create>();
		var nameInput = cut.Find("#name");
		var descriptionInput = cut.Find("#description");
		var form = cut.Find("form");
		var submitButton = cut.Find("button[type='submit']");

		var tcs = new TaskCompletionSource<Result>();
		_categoryServiceSub.CreateAsync(Arg.Any<CategoryDto>()).Returns(tcs.Task);

		// Act
		nameInput.Change("Test");
		descriptionInput.Change("Test Description");
		form.Submit();

		// Assert
		submitButton.HasAttribute("disabled").Should().BeTrue();
		cut.Markup.Should().Contain("Creating");

		tcs.SetResult(Result.Ok()); // Complete the task
	}

	[Fact]
	public void Cancel_Link_Navigates_To_Categories_List()
	{
		// Arrange
		var cut = Render<Create>();

		// Assert
		var cancelLink = cut.Find("a[href='/categories']");
		cancelLink.Should().NotBeNull();
		cancelLink.TextContent.Should().Contain("Cancel");
	}

}