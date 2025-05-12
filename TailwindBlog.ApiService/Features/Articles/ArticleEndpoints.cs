// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleEndpoints.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TailwindBlog.ApiService.Features.Articles.Commands;
using TailwindBlog.ApiService.Features.Articles.Queries;
using TailwindBlog.Domain.Entities;

namespace TailwindBlog.ApiService.Features.Articles;

public static class ArticleEndpoints
{
	public static void MapArticleEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/api/articles")
				.WithTags("Articles")
				.WithOpenApi()
				.RequireAuthorization();

		group.MapGet("/", async ([FromServices] ISender sender, CancellationToken ct) =>
		{
			var query = new GetArticlesQuery();
			var result = await sender.Send(query, ct);
			return result.Success ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
		})
		.WithName("GetArticles")
		.WithDisplayName("Get all articles")
		.Produces<IEnumerable<Article>>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status400BadRequest);

		group.MapGet("/{id}", async ([FromRoute] string id, [FromServices] ISender sender, CancellationToken ct) =>
		{
			if (!ObjectId.TryParse(id, out var articleId))
			{
				return Results.BadRequest("Invalid article ID format");
			}

			var query = new GetArticleByIdQuery(articleId);
			var result = await sender.Send(query, ct);
			return result.Success ? Results.Ok(result.Value) : Results.NotFound(result.Error);
		})
		.WithName("GetArticleById")
		.WithDisplayName("Get article by ID")
		.Produces<Article>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status404NotFound)
		.Produces(StatusCodes.Status400BadRequest);

		group.MapPost("/", async ([FromBody] Article article, [FromServices] ISender sender, CancellationToken ct) =>
		{
			var command = new CreateArticleCommand(article);
			var result = await sender.Send(command, ct); return result.Success && result.Value is not null
							? Results.Created($"/api/articles/{result.Value.Id}", result.Value)
							: Results.BadRequest(result.Error ?? "Failed to create article");
		})
		.WithName("CreateArticle")
		.WithDisplayName("Create a new article")
		.Produces<Article>(StatusCodes.Status201Created)
		.Produces(StatusCodes.Status400BadRequest);

		group.MapPut("/{id}", async ([FromRoute] string id, [FromBody] Article article, [FromServices] ISender sender, CancellationToken ct) =>
		{
			if (!ObjectId.TryParse(id, out var articleId) || article.Id != articleId)
			{
				return Results.BadRequest("Invalid article ID format or ID mismatch");
			}

			var command = new UpdateArticleCommand(article);
			var result = await sender.Send(command, ct);
			return result.Success ? Results.Ok(result.Value) : Results.NotFound(result.Error);
		})
		.WithName("UpdateArticle")
		.WithDisplayName("Update an existing article")
		.Produces<Article>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status404NotFound)
		.Produces(StatusCodes.Status400BadRequest);

		group.MapDelete("/{id}", async ([FromRoute] string id, [FromServices] ISender sender, CancellationToken ct) =>
		{
			if (!ObjectId.TryParse(id, out var articleId))
			{
				return Results.BadRequest("Invalid article ID format");
			}

			var command = new DeleteArticleCommand(articleId);
			var result = await sender.Send(command, ct);
			return result.Success ? Results.NoContent() : Results.NotFound(result.Error);
		})
		.WithName("DeleteArticle")
		.WithDisplayName("Delete an article")
		.Produces(StatusCodes.Status204NoContent)
		.Produces(StatusCodes.Status404NotFound)
		.Produces(StatusCodes.Status400BadRequest);
	}
}
