// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DeleteCategory.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.ApiService.Features.Categories.Commands;

public record DeleteCategoryCommand(ObjectId CategoryId) : IRequest<Result<bool>>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
	private readonly ICategoryRepository _categoryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
	{
		_categoryRepository = categoryRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
			if (category is null)
			{
				return Result.Fail<bool>($"Category with ID {request.CategoryId} not found");
			}

			_categoryRepository.Remove(category);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result.Ok(true);
		}
		catch (Exception ex)
		{
			return Result.Fail<bool>($"Failed to delete category: {ex.Message}");
		}
	}
}
