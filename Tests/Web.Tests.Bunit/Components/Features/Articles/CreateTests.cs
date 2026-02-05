// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CreateTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Features.Articles;

/// <summary>
///   Unit tests for <see cref="Create" /> (Articles Create Page).
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(Create))]
public class CreateTests : BunitContext
{

	private readonly IArticleService _articleServiceSub = Substitute.For<IArticleService>();

	public CreateTests()
	{
		Services.AddSingleton(_articleServiceSub);
		Services.AddCascadingAuthenticationState();
		Services.AddAuthorization();
	}

	[Fact]
	public void Renders_Form()
	{
		// Arrange
		var cut = Render<Create>();

		// Act
		// (No action needed, render)

		// Assert
		cut.Markup.Should().Contain("Title");
		cut.Markup.Should().Contain("Introduction");
	}

	[Fact]
	public async Task Submits_Valid_Form_And_Navigates_On_Success()
	{
		// Arrange
		_articleServiceSub.CreateAsync(Arg.Any<ArticleDto>()).Returns(Result.Ok(new ArticleDto()));
		var nav = Services.GetRequiredService<BunitNavigationManager>();
		var cut = Render<Create>();

		// Act
		await cut.Find("form").SubmitAsync();

		// Assert
		nav.Uri.Should().EndWith("/articles");
	}

	[Fact]
	public async Task Displays_Error_On_Failed_Submit()
	{
		// Arrange
		_articleServiceSub.CreateAsync(Arg.Any<ArticleDto>()).Returns(Result.Fail<ArticleDto>("Create failed"));
		var cut = Render<Create>();

		// Act
		await cut.Find("form").SubmitAsync();

		// Assert
		cut.Markup.Should().Contain("Create failed");
	}

	[Fact]
	public void Cancel_Button_Navigates_To_List()
	{
		// Arrange
		var nav = Services.GetRequiredService<BunitNavigationManager>();
		var cut = Render<Create>();

		// Act
		cut.Find("button.btn-light").Click();

		// Assert
		nav.Uri.Should().EndWith("/articles");
	}

}