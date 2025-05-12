// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     ArticleService.cs
// Project Name:  TailwindBlog.Web
// =======================================================

namespace TailwindBlog.Web.Services;

public class ArticleService : IArticleService
{
	private readonly HttpClient _httpClient;

	public ArticleService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<IEnumerable<Article>> GetArticlesAsync(CancellationToken cancellationToken = default)
	{
		var articles = await _httpClient.GetFromJsonAsync<IEnumerable<Article>>("api/articles", cancellationToken);
		return articles ?? Enumerable.Empty<Article>();
	}

	public async Task<Article?> GetArticleByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		return await _httpClient.GetFromJsonAsync<Article>($"api/articles/{id}", cancellationToken);
	}

	public async Task<Result<Article>> CreateArticleAsync(Article article, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.PostAsJsonAsync("api/articles", article, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			return Result<Article>.Fail("Failed to create article");
		}

		var createdArticle = await response.Content.ReadFromJsonAsync<Article>(cancellationToken: cancellationToken);
		return Result<Article>.Ok(createdArticle!);
	}

	public async Task<Result<Article>> UpdateArticleAsync(string id, Article article, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.PutAsJsonAsync($"api/articles/{id}", article, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			return Result<Article>.Fail("Failed to update article");
		}

		var updatedArticle = await response.Content.ReadFromJsonAsync<Article>(cancellationToken: cancellationToken);
		return Result<Article>.Ok(updatedArticle!);
	}

	public async Task<Result> DeleteArticleAsync(string id, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.DeleteAsync($"api/articles/{id}", cancellationToken);

		return response.IsSuccessStatusCode
				? Result.Ok()
				: Result.Fail("Failed to delete article");
	}
}
