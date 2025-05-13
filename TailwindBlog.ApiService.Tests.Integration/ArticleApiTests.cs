// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleApiTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService.Tests.Integration
// =======================================================

namespace TailwindBlog.ApiService.Tests.Integration;

[ExcludeFromCodeCoverage]
public class ArticleApiTests : ApiTestBase
{
	[Fact]
	public async Task GetAll_ShouldReturnEmptyList_WhenNoArticlesExist()
	{
		// Act
		var response = await _client.GetAsync("/api/articles");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var articles = await response.Content.ReadFromJsonAsync<IEnumerable<Article>>();
		articles.Should().NotBeNull();
		articles.Should().BeEmpty();
	}

	[Fact]
	public async Task GetById_ShouldReturn404_WhenArticleDoesNotExist()
	{
		// Arrange
		var invalidId = ObjectId.GenerateNewId();

		// Act
		var response = await _client.GetAsync($"/api/articles/{invalidId}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Post_ShouldCreateArticle_WhenDataIsValid()
	{
		// Arrange
		var article = new Article
		{
			Title = "Test Article",
			Introduction = "Test Introduction",
			CoverImageUrl = "http://example.com/image.jpg",
			Author = new AppUserModel { Id = "user1", UserName = "testuser" },
			IsPublished = true
		};

		// Act
		var createResponse = await _client.PostAsJsonAsync("/api/articles", article);

		// Assert
		createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
		createResponse.Headers.Location.Should().NotBeNull();

		// Verify created article
		var getResponse = await _client.GetAsync(createResponse.Headers.Location);
		getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

		var createdArticle = await getResponse.Content.ReadFromJsonAsync<Article>();
		createdArticle.Should().NotBeNull();
		createdArticle!.Title.Should().Be(article.Title);
		createdArticle.Introduction.Should().Be(article.Introduction);
		createdArticle.CoverImageUrl.Should().Be(article.CoverImageUrl);
		createdArticle.Author.Id.Should().Be(article.Author.Id);
		createdArticle.IsPublished.Should().Be(article.IsPublished);
		createdArticle.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
	}

	[Fact]
	public async Task Put_ShouldUpdateArticle_WhenDataIsValid()
	{
		// Arrange 
		var article = new Article
		{
			Title = "Original Title",
			Introduction = "Original Intro",
			CoverImageUrl = "http://example.com/old.jpg",
			Author = new AppUserModel { Id = "user1", UserName = "testuser" },
			IsPublished = false
		};

		var createResponse = await _client.PostAsJsonAsync("/api/articles", article);
		var location = createResponse.Headers.Location;

		var update = new Article
		{
			Title = "Updated Title",
			Introduction = "Updated Intro",
			CoverImageUrl = "http://example.com/new.jpg",
			Author = article.Author,
			IsPublished = true
		};

		// Act
		var updateResponse = await _client.PutAsJsonAsync(location, update);

		// Assert
		updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

		// Verify update
		var getResponse = await _client.GetAsync(location);
		var updatedArticle = await getResponse.Content.ReadFromJsonAsync<Article>();

		updatedArticle.Should().NotBeNull();
		updatedArticle!.Title.Should().Be(update.Title);
		updatedArticle.Introduction.Should().Be(update.Introduction);
		updatedArticle.CoverImageUrl.Should().Be(update.CoverImageUrl);
		updatedArticle.IsPublished.Should().Be(update.IsPublished);
		updatedArticle.ModifiedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
	}

	[Fact]
	public async Task Delete_ShouldRemoveArticle_WhenArticleExists()
	{
		// Arrange
		var article = new Article
		{
			Title = "Article to Delete",
			Introduction = "Will be deleted",
			CoverImageUrl = "http://example.com/delete.jpg",
			Author = new AppUserModel { Id = "user1", UserName = "testuser" }
		};

		var createResponse = await _client.PostAsJsonAsync("/api/articles", article);
		var location = createResponse.Headers.Location;

		// Act
		var deleteResponse = await _client.DeleteAsync(location);

		// Assert
		deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

		// Verify deletion
		var getResponse = await _client.GetAsync(location);
		getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Post_ShouldReturn400_WhenDataIsInvalid()
	{
		// Arrange
		var invalidArticle = Article.Empty; // Missing required fields

		// Act
		var response = await _client.PostAsJsonAsync("/api/articles", invalidArticle);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task GetByAuthor_ShouldReturnOnlyAuthorArticles()
	{
		// Arrange
		var author1 = new AppUserModel { Id = "user1", UserName = "author1" };
		var author2 = new AppUserModel { Id = "user2", UserName = "author2" };

		var articles = new[]
		{
						new Article { Title = "Author 1 Article 1", Author = author1 },
						new Article { Title = "Author 1 Article 2", Author = author1 },
						new Article { Title = "Author 2 Article", Author = author2 }
				};

		foreach (var article in articles)
		{
			await _client.PostAsJsonAsync("/api/articles", article);
		}

		// Act
		var response = await _client.GetAsync($"/api/articles/author/{author1.Id}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var authorArticles = await response.Content.ReadFromJsonAsync<List<Article>>();
		authorArticles.Should().NotBeNull();
		authorArticles.Should().HaveCount(2);
		authorArticles.Should().OnlyContain(a => a.Author.Id == author1.Id);
	}
}
