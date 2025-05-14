// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Repository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TailwindBlog.Domain.Abstractions;
using TailwindBlog.Domain.Interfaces;

namespace TailwindBlog.Persistence.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
	where TEntity : Entity
{
	protected readonly AppDbContext Context;
	protected readonly DbSet<TEntity> DbSet;

	protected Repository(AppDbContext context)
	{
		Context = context;
		DbSet = context.Set<TEntity>();
	}

	public void Add(TEntity entity)
	{
		DbSet.Add(entity);
	}

	public async Task AddRangeAsync(IEnumerable<TEntity> entities)
	{
		await DbSet.AddRangeAsync(entities);
	}

	public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
	{
		return await DbSet.AnyAsync(predicate);
	}

	public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> criteria)
	{
		return await DbSet.Where(criteria).ToListAsync();
	}

	public async Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> predicate)
	{
		return await DbSet.FirstOrDefaultAsync(predicate);
	}

	public async Task<TEntity?> GetByIdAsync(Guid id)
	{
		// MongoDB EF Core does not support FindAsync; use FirstOrDefaultAsync
		return await DbSet.FirstOrDefaultAsync(e => e.Id == id);
	}

	public void Remove(TEntity entity)
	{
		DbSet.Remove(entity);
	}

	public void RemoveRange(IEnumerable<TEntity> entities)
	{
		DbSet.RemoveRange(entities);
	}

	public async Task<IEnumerable<TEntity>> ToListAsync()
	{
		return await DbSet.ToListAsync();
	}

	public void Update(TEntity entity)
	{
		DbSet.Update(entity);
	}
}