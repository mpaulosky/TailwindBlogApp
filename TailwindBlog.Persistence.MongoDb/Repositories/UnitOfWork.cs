// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     UnitOfWork.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{

	private readonly AppDbContext _Context;

	public UnitOfWork(AppDbContext context)
	{

		_Context = context;

	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{

		return await _Context.SaveChangesAsync(cancellationToken);

	}

}
