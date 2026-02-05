// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DetailsTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Features.Articles;

/// <summary>
///   Unit tests for <see cref="Details" />
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(Details))]
public class DetailsTests : BunitContext
{

	private readonly IArticleService _articleServiceSub = Substitute.For<IArticleService>();

	public DetailsTests()
	{
		Services.AddSingleton(_articleServiceSub);
		Services.AddCascadingAuthenticationState();
		Services.AddAuthorization();
	}


	[Fact]
	public void RendersNotFound_WhenArticleIsNull()
	{
		// Arrange
		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, Guid.CreateVersion7()));

		// Act
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, null);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Article not found");
	}

	[Fact]
	public void RendersArticleDetails_WhenArticleIsPresent()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));
		var cut = Render<Details>(parameters => parameters.Add(p => p.Id, articleDto.Id));
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, articleDto);

		// Act
		cut.Render();

		// Assert
		cut.Markup.Should().Contain(articleDto.Title);
		cut.Markup.Should().Contain(articleDto.Introduction);
		cut.Markup.Should().Contain(articleDto.Author.UserName);
		cut.Markup.Should().Contain(articleDto.Category.Name);
	}

	[Fact]
	public void RendersArticleContent_AsMarkupString()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		articleDto.Content = "<p>Test HTML content</p>";
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));
		var cut = Render<Details>(parameters => parameters.Add(p => p.Id, articleDto.Id));
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, articleDto);

		// Act
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("<p>Test HTML content</p>");
	}

	[Fact]
	public void DisplaysCorrectPublishedStatus()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		articleDto.IsPublished = true;
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));
		var cut = Render<Details>(parameters => parameters.Add(p => p.Id, articleDto.Id));
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, articleDto);

		// Act
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Published:</strong> Yes");
	}

	[Fact]
	public void DisplaysPublishedDate_WhenPresent()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		articleDto.IsPublished = true;
		articleDto.PublishedOn = DateTime.Now.Date;
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));
		var cut = Render<Details>(parameters => parameters.Add(p => p.Id, articleDto.Id));
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, articleDto);

		// Act
		cut.Render();

		// Assert
		cut.Markup.Should().Contain(articleDto.PublishedOn?.ToString("d"));
	}

	[Fact]
	public void HasCorrectNavigationButtons()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));
		var cut = Render<Details>(parameters => parameters.Add(p => p.Id, articleDto.Id));
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, articleDto);

		// Act
		cut.Render();

		// Assert
		cut.Find("button.btn-secondary").Should().NotBeNull();
		cut.Find("button.btn-light").Should().NotBeNull();
	}

	[Fact]
	public void NavigatesToEditPage_WhenEditButtonClicked()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));
		var navigationManager = Services.GetRequiredService<BunitNavigationManager>();
		var cut = Render<Details>(parameters => parameters.Add(p => p.Id, articleDto.Id));
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, articleDto);
		cut.Render();

		// Act
		cut.Find("button.btn-secondary").Click();

		// Assert
		navigationManager.Uri.Should().EndWith($"/articles/edit/{articleDto.Id}");
	}

	[Fact]
	public void NavigatesToListPage_WhenBackButtonClicked()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));
		var navigationManager = Services.GetRequiredService<BunitNavigationManager>();
		var cut = Render<Details>(parameters => parameters.Add(p => p.Id, articleDto.Id));
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, articleDto);
		cut.Render();

		// Act
		cut.Find("button.btn-light").Click();


		// Assert
		navigationManager.Uri.Should().EndWith("/articles");
	}

	[Fact]
	public void DisplaysNotPublished_WhenArticleIsNotPublished()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		articleDto.IsPublished = false;
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, articleDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, articleDto);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Published:</strong> No");
	}

	[Fact]
	public void DisplaysNullPublishedDate_WhenNotPublished()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		articleDto.IsPublished = false;
		articleDto.PublishedOn = null;
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, articleDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, articleDto);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Published On:</strong> ");
		cut.Markup.Should().NotContain("Published On:</strong> null");
	}

	[Fact]
	public void HandlesEmptyGuid()
	{
		// Arrange
		_articleServiceSub.GetAsync(Guid.Empty).Returns(Result.Fail<ArticleDto>("Not found"));

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, Guid.Empty));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Article not found");
	}

	[Fact]
	public void HandlesServiceException_Gracefully()
	{
		// Arrange
		var articleId = Guid.CreateVersion7();

		_articleServiceSub.GetAsync(articleId)
				.Returns(Task.FromException<Result<ArticleDto>>(new Exception("Service error")));

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, articleId));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Article not found");
	}

	[Fact]
	public void DisplaysCoverImage_WithCorrectAttributes()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		articleDto.CoverImageUrl = "https://example.com/image.jpg";
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));

		var cut = Render<Details>(parameters => parameters
				.Add(p => p.Id, articleDto.Id));

		// Simulate loading complete
		cut.Instance.GetType().GetProperty("_isLoading")?.SetValue(cut.Instance, false);
		cut.Instance.GetType().GetProperty("_article")?.SetValue(cut.Instance, articleDto);
		cut.Render();

		// Assert
		var imgElement = cut.Find("img.card-img-top");
		imgElement.Should().NotBeNull();
		imgElement.Attributes["src"]?.Value.Should().Be(articleDto.CoverImageUrl);
		imgElement.Attributes["alt"]?.Value.Should().Be("Cover");
	}

	[Fact]
	public async Task CallsArticleService_OnInitializedAsync()
	{
		// Arrange
		var articleDto = FakeArticleDto.GetNewArticleDto(true);
		_articleServiceSub.GetAsync(articleDto.Id).Returns(Result.Ok(articleDto));

		// Act
		Render<Details>(parameters => parameters
				.Add(p => p.Id, articleDto.Id));

		// Assert
		await _articleServiceSub.Received(1).GetAsync(Arg.Is<Guid>(id => id == articleDto.Id));
	}

}