// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CreateCategory.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.ApiService.Features.Categories.Commands;

public record CreateCategoryCommand(Category Category) : IRequest<Result<Category>>;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Category>>
{
	private readonly ICategoryRepository _categoryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
	{
		_categoryRepository = categoryRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		try
		{
			_categoryRepository.Add(request.Category);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result<Category>.Ok(request.Category);
		}
		catch (Exception ex)
		{
			return Result<Category>.Fail($"Failed to create category: {ex.Message}");
		}
	}
}
