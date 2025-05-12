// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleEndpointsTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService.Tests.Integration
// =======================================================

namespace TailwindBlog.ApiService.Tests.Integration.Endpoints;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Program))]
public class ArticleEndpointsTests : ApiTestBase
{
	[Fact]
	public async Task Get_Articles_Returns_Success()
	{
		// Act
		var response = await _client.GetAsync("/articles");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var articles = await response.Content.ReadFromJsonAsync<IEnumerable<Article>>();
		articles.Should().NotBeNull();
	}

	[Fact]
	public async Task Get_Article_By_Id_Returns_NotFound_For_Invalid_Id()
	{
		// Arrange
		var invalidId = ObjectId.GenerateNewId().ToString();

		// Act
		var response = await _client.GetAsync($"/articles/{invalidId}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Create_Article_Returns_Success()
	{
		// Arrange
		var article = new Article
		{
			Title = "Test Article",
			Introduction = "Test Introduction",
			UrlSlug = "test-article",
			Author = AppUserModel.Empty,
			CreatedOn = DateTime.UtcNow
		};

		// Act
		var response = await _client.PostAsJsonAsync("/articles", article);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.Created);
		var location = response.Headers.Location;
		location.Should().NotBeNull();
	}

	[Fact]
	public async Task Update_Article_Returns_Success()
	{
		// Arrange
		var article = new Article
		{
			Title = "Test Article",
			Introduction = "Test Introduction",
			UrlSlug = "test-article",
			Author = AppUserModel.Empty,
			CreatedOn = DateTime.UtcNow
		};
		var createResponse = await _client.PostAsJsonAsync("/articles", article);
		var location = createResponse.Headers.Location;

		article.Title = "Updated Title";

		// Act
		var response = await _client.PutAsJsonAsync(location, article);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task Delete_Article_Returns_Success()
	{
		// Arrange
		var article = new Article
		{
			Title = "Test Article",
			Introduction = "Test Introduction",
			UrlSlug = "test-article",
			Author = AppUserModel.Empty,
			CreatedOn = DateTime.UtcNow
		};
		var createResponse = await _client.PostAsJsonAsync("/articles", article);
		var location = createResponse.Headers.Location;

		// Act
		var response = await _client.DeleteAsync(location);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}
}
