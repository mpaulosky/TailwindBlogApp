// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryEndpoints.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using TailwindBlog.ApiService.Features.Categories.Commands;
using TailwindBlog.ApiService.Features.Categories.Queries;

namespace TailwindBlog.ApiService.Features.Categories;

public static class CategoryEndpoints
{
	public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/api/categories")
				.WithTags("Categories")
				.WithOpenApi()
				.RequireAuthorization();

		group.MapGet("/", async ([FromServices] ISender sender, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default) =>
		{
			var query = new GetCategoriesQuery(pageNumber, pageSize);
			var result = await sender.Send(query, ct);
			return result.Success ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
		})
		.WithName("GetCategories")
		.WithDisplayName("Get all categories")
		.Produces<PaginationModel<Category>>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status400BadRequest);

		group.MapGet("/{id}", async ([FromRoute] string id, [FromServices] ISender sender, CancellationToken ct) =>
		{
			if (!ObjectId.TryParse(id, out var categoryId))
			{
				return Results.BadRequest("Invalid category ID format");
			}

			var query = new GetCategoryByIdQuery(categoryId);
			var result = await sender.Send(query, ct);
			return result.Success ? Results.Ok(result.Value) : Results.NotFound(result.Error);
		})
		.WithName("GetCategoryById")
		.WithDisplayName("Get category by ID")
		.Produces<Category>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status404NotFound)
		.Produces(StatusCodes.Status400BadRequest);

		group.MapPost("/", async ([FromBody] Category category, [FromServices] ISender sender, CancellationToken ct) =>
		{
			var command = new CreateCategoryCommand(category);
			var result = await sender.Send(command, ct); return result.Success && result.Value is not null
													? Results.Created($"/api/categories/{result.Value.Id}", result.Value)
													: Results.BadRequest(result.Error ?? "Failed to create category");
		})
		.WithName("CreateCategory")
		.WithDisplayName("Create a new category")
		.Produces<Category>(StatusCodes.Status201Created)
		.Produces(StatusCodes.Status400BadRequest);

		group.MapPut("/{id}", async ([FromRoute] string id, [FromBody] Category category, [FromServices] ISender sender, CancellationToken ct) =>
		{
			if (!ObjectId.TryParse(id, out var categoryId) || category.Id != categoryId)
			{
				return Results.BadRequest("Invalid category ID format or ID mismatch");
			}

			var command = new UpdateCategoryCommand(category);
			var result = await sender.Send(command, ct);
			return result.Success ? Results.Ok(result.Value) : Results.NotFound(result.Error);
		})
		.WithName("UpdateCategory")
		.WithDisplayName("Update an existing category")
		.Produces<Category>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status404NotFound)
		.Produces(StatusCodes.Status400BadRequest);

		group.MapDelete("/{id}", async ([FromRoute] string id, [FromServices] ISender sender, CancellationToken ct) =>
		{
			if (!ObjectId.TryParse(id, out var categoryId))
			{
				return Results.BadRequest("Invalid category ID format");
			}

			var command = new DeleteCategoryCommand(categoryId);
			var result = await sender.Send(command, ct);
			return result.Success ? Results.NoContent() : Results.NotFound(result.Error);
		})
		.WithName("DeleteCategory")
		.WithDisplayName("Delete a category")
		.Produces(StatusCodes.Status204NoContent)
		.Produces(StatusCodes.Status404NotFound)
		.Produces(StatusCodes.Status400BadRequest);
	}
}
