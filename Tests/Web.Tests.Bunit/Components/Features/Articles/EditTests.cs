// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     EditTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Features.Articles;

/// <summary>
///   Unit tests for <see cref="Edit" /> (Articles Edit Page).
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(Edit))]
public class EditTests : BunitContext
{

	private readonly IArticleService _articleServiceSub = Substitute.For<IArticleService>();

	public EditTests()
	{

		Services.AddSingleton(_articleServiceSub);
		Services.AddCascadingAuthenticationState();
		Services.AddAuthorization();

	}

	[Fact]
	public void Renders_NotFound_When_Article_Is_Null()
	{

		// Arrange
		var id = Guid.NewGuid();
		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, id));

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_article", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, null);

		// Act
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Article not found");

	}

	[Fact]
	public void Renders_Form_With_Article_Data()
	{

		// Arrange
		var article = FakeArticleDto.GetNewArticleDto(true);
		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, article.Id));

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_article", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, article);

		// Act
		cut.Render();

		// Assert
		cut.Markup.Should().Contain(article.Title);
		cut.Markup.Should().Contain(article.Introduction);

	}

	[Fact]
	public async Task Submits_Valid_Form_And_Navigates_On_Success()
	{

		// Arrange
		var article = FakeArticleDto.GetNewArticleDto(true);
		_articleServiceSub.UpdateAsync(article).Returns(Result.Ok(article));
		var nav = Services.GetRequiredService<BunitNavigationManager>();
		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, article.Id));

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_article", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, article);

		cut.Render();

		// Act
		await cut.Find("form").SubmitAsync();

		// Assert
		nav.Uri.Should().EndWith("/categories");

	}

	[Fact]
	public async Task Displays_Error_On_Failed_Submit()
	{

		// Arrange
		var article = FakeArticleDto.GetNewArticleDto(true);
		_articleServiceSub.UpdateAsync(article).Returns(Result.Fail<ArticleDto>("Update failed"));
		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, article.Id));

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_article", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, article);

		cut.Render();

		// Act
		await cut.Find("form").SubmitAsync();

		// Assert
		cut.Markup.Should().Contain("Update failed");

	}

	[Fact]
	public void Cancel_Button_Navigates_To_List()
	{

		// Arrange
		var article = FakeArticleDto.GetNewArticleDto(true);
		var nav = Services.GetRequiredService<BunitNavigationManager>();
		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, article.Id));

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_article", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, article);

		cut.Render();

		// Act
		cut.Find("button.btn-light").Click();

		// Assert
		nav.Uri.Should().EndWith("/articles");

	}

}