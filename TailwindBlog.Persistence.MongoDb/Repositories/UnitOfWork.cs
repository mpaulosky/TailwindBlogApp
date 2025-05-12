// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     UnitOfWork.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{

	private readonly AppDbContext _context;

	public UnitOfWork(AppDbContext context)
	{

		_context = context;

	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{

		return await _context.SaveChangesAsync(cancellationToken);

	}

}