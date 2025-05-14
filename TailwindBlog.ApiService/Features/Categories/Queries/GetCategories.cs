// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     GetCategories.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using TailwindBlog.Domain.Extensions;

namespace TailwindBlog.ApiService.Features.Categories.Queries;

public record GetCategoriesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PaginationModel<Category>>>;

public sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<PaginationModel<Category>>>
{
	private readonly ICategoryRepository _categoryRepository;

	public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<Result<PaginationModel<Category>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var query = (await _categoryRepository.GetAllAsync()).AsQueryable();
			var totalCount = query.Count();
			var categories = query
					.Paginate(request.PageNumber, request.PageSize)
					.ToList();

			var paginatedResult = PaginationModel<Category>.Create(
					categories,
					totalCount,
					request.PageNumber,
					request.PageSize);

			return Result<PaginationModel<Category>>.Ok(paginatedResult);
		}
		catch (Exception ex)
		{
			return Result<PaginationModel<Category>>.Fail($"Failed to retrieve categories: {ex.Message}");
		}
	}
}
