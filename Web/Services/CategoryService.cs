// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     CategoryService.cs
// Project Name:  Web
// =======================================================

namespace Web.Services;

public class CategoryService : ICategoryService
{
	private readonly HttpClient _httpClient;

	public CategoryService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default)
	{
		var categories = await _httpClient.GetFromJsonAsync<IEnumerable<Category>>("api/categories", cancellationToken);
		return categories ?? Enumerable.Empty<Category>();
	}

	public async Task<Category?> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		return await _httpClient.GetFromJsonAsync<Category>($"api/categories/{id}", cancellationToken);
	}

	public async Task<Result<Category>> CreateCategoryAsync(Category category, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.PostAsJsonAsync("api/categories", category, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			return Result<Category>.Fail("Failed to create category");
		}

		var createdCategory = await response.Content.ReadFromJsonAsync<Category>(cancellationToken: cancellationToken);
		return Result<Category>.Ok(createdCategory!);
	}

	public async Task<Result<Category>> UpdateCategoryAsync(string id, Category category, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.PutAsJsonAsync($"api/categories/{id}", category, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			return Result<Category>.Fail("Failed to update category");
		}

		var updatedCategory = await response.Content.ReadFromJsonAsync<Category>(cancellationToken: cancellationToken);
		return Result<Category>.Ok(updatedCategory!);
	}

	public async Task<Result> DeleteCategoryAsync(string id, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.DeleteAsync($"api/categories/{id}", cancellationToken);

		return response.IsSuccessStatusCode
				? Result.Ok()
				: Result.Fail("Failed to delete category");
	}
}
