// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     UpdateCategory.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.ApiService.Features.Categories.Commands;

public record UpdateCategoryCommand(Category Category) : IRequest<Result<Category>>;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<Category>>
{
	private readonly ICategoryRepository _categoryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
	{
		_categoryRepository = categoryRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Category>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
	{
		try
		{
			if (await _categoryRepository.GetByIdAsync(request.Category.Id) is null)
			{
				return Result<Category>.Fail($"Category with ID {request.Category.Id} not found");
			}

			_categoryRepository.Update(request.Category);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result<Category>.Ok(request.Category);
		}
		catch (Exception ex)
		{
			return Result<Category>.Fail($"Failed to update category: {ex.Message}");
		}
	}
}
