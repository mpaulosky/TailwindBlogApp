// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryEndpointsTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService.Tests.Integration
// =======================================================

namespace TailwindBlog.ApiService.Tests.Integration.Endpoints;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Program))]
public class CategoryEndpointsTests : ApiTestBase
{
	[Fact]
	public async Task Get_Categories_Returns_Success()
	{
		// Act
		var response = await _client.GetAsync("/categories");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var categories = await response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
		categories.Should().NotBeNull();
	}

	[Fact]
	public async Task Get_Category_By_Id_Returns_NotFound_For_Invalid_Id()
	{
		// Arrange
		var invalidId = ObjectId.GenerateNewId().ToString();

		// Act
		var response = await _client.GetAsync($"/categories/{invalidId}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Create_Category_Returns_Success()
	{
		// Arrange
		var category = new Category
		{
			Name = "Test Category",
			Description = "Test Description"
		};

		// Act
		var response = await _client.PostAsJsonAsync("/categories", category);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.Created);
		var location = response.Headers.Location;
		location.Should().NotBeNull();
	}

	[Fact]
	public async Task Update_Category_Returns_Success()
	{
		// Arrange
		var category = new Category
		{
			Name = "Test Category",
			Description = "Test Description"
		};
		var createResponse = await _client.PostAsJsonAsync("/categories", category);
		var location = createResponse.Headers.Location;

		category.Name = "Updated Category";

		// Act
		var response = await _client.PutAsJsonAsync(location, category);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task Delete_Category_Returns_Success()
	{
		// Arrange
		var category = new Category
		{
			Name = "Test Category",
			Description = "Test Description"
		};
		var createResponse = await _client.PostAsJsonAsync("/categories", category);
		var location = createResponse.Headers.Location;

		// Act
		var response = await _client.DeleteAsync(location);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}
}
