// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     GetCategoryById.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.ApiService.Features.Categories.Queries;

public record GetCategoryByIdQuery(ObjectId CategoryId) : IRequest<Result<Category>>;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<Category>>
{
	private readonly ICategoryRepository _categoryRepository;

	public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<Result<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
			return category is not null
					? Result<Category>.Ok(category)
					: Result<Category>.Fail($"Category with ID {request.CategoryId} not found");
		}
		catch (Exception ex)
		{
			return Result<Category>.Fail($"Failed to retrieve category: {ex.Message}");
		}
	}
}
