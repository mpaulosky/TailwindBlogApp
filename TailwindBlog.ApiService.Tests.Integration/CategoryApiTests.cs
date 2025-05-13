// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryApiTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService.Tests.Integration
// =======================================================

namespace TailwindBlog.ApiService.Tests.Integration;

[ExcludeFromCodeCoverage]
public class CategoryApiTests : ApiTestBase
{
	[Fact]
	public async Task GetAll_ShouldReturnEmptyList_WhenNoCategoriesExist()
	{
		// Act
		var response = await _client.GetAsync("/api/categories");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var categories = await response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
		categories.Should().NotBeNull();
		categories.Should().BeEmpty();
	}

	[Fact]
	public async Task GetById_ShouldReturn404_WhenCategoryDoesNotExist()
	{
		// Arrange
		var invalidId = ObjectId.GenerateNewId();

		// Act
		var response = await _client.GetAsync($"/api/categories/{invalidId}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Post_ShouldCreateCategory_WhenDataIsValid()
	{
		// Arrange
		var category = new Category
		{
			Name = "Test Category",
			Description = "Test Description",
			UrlSlug = "test-category",
			ShowOnNavigation = true
		};

		// Act
		var createResponse = await _client.PostAsJsonAsync("/api/categories", category);

		// Assert
		createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
		createResponse.Headers.Location.Should().NotBeNull();

		// Verify created category
		var getResponse = await _client.GetAsync(createResponse.Headers.Location);
		getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

		var createdCategory = await getResponse.Content.ReadFromJsonAsync<Category>();
		createdCategory.Should().NotBeNull();
		createdCategory!.Name.Should().Be(category.Name);
		createdCategory.Description.Should().Be(category.Description);
		createdCategory.UrlSlug.Should().Be(category.UrlSlug);
		createdCategory.ShowOnNavigation.Should().Be(category.ShowOnNavigation);
	}

	[Fact]
	public async Task Put_ShouldUpdateCategory_WhenDataIsValid()
	{
		// Arrange
		var category = new Category
		{
			Name = "Original Name",
			Description = "Original Description",
			UrlSlug = "original-slug",
			ShowOnNavigation = false
		};

		var createResponse = await _client.PostAsJsonAsync("/api/categories", category);
		var location = createResponse.Headers.Location;

		var update = new Category
		{
			Name = "Updated Name",
			Description = "Updated Description",
			UrlSlug = "updated-slug",
			ShowOnNavigation = true
		};

		// Act
		var updateResponse = await _client.PutAsJsonAsync(location, update);

		// Assert
		updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

		// Verify update
		var getResponse = await _client.GetAsync(location);
		var updatedCategory = await getResponse.Content.ReadFromJsonAsync<Category>();

		updatedCategory.Should().NotBeNull();
		updatedCategory!.Name.Should().Be(update.Name);
		updatedCategory.Description.Should().Be(update.Description);
		updatedCategory.UrlSlug.Should().Be(update.UrlSlug);
		updatedCategory.ShowOnNavigation.Should().Be(update.ShowOnNavigation);
	}

	[Fact]
	public async Task Delete_ShouldRemoveCategory_WhenCategoryExists()
	{
		// Arrange
		var category = new Category
		{
			Name = "Category to Delete",
			Description = "Will be deleted",
			UrlSlug = "delete-me",
			ShowOnNavigation = true
		};

		var createResponse = await _client.PostAsJsonAsync("/api/categories", category);
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
		var invalidCategory = Category.Empty; // Missing required fields

		// Act
		var response = await _client.PostAsJsonAsync("/api/categories", invalidCategory);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task GetNavigationCategories_ShouldOnlyReturnCategoriesMarkedForNavigation()
	{
		// Arrange
		var categories = new[]
		{
						new Category { Name = "Nav Category 1", ShowOnNavigation = true },
						new Category { Name = "Nav Category 2", ShowOnNavigation = true },
						new Category { Name = "Hidden Category", ShowOnNavigation = false }
				};

		foreach (var category in categories)
		{
			await _client.PostAsJsonAsync("/api/categories", category);
		}

		// Act
		var response = await _client.GetAsync("/api/categories/navigation");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var navCategories = await response.Content.ReadFromJsonAsync<List<Category>>();
		navCategories.Should().NotBeNull();
		navCategories.Should().HaveCount(2);
		navCategories.Should().OnlyContain(c => c.ShowOnNavigation);
	}
}
