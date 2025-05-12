// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Repository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

public abstract class Repository<TEntity>
		where TEntity : Entity
{

	protected readonly AppDbContext Context;

	protected Repository(AppDbContext context)
	{

		Context = context;

	}

	public void Add(TEntity entity)
	{

		Context.Set<TEntity>().Add(entity);

	}

	public async Task AddRangeAsync(IEnumerable<TEntity> entities)
	{

		await Context.Set<TEntity>().AddRangeAsync(entities);

	}

	public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
	{

		return await Context.Set<TEntity>().AnyAsync(predicate);

	}

	public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
	{

		return await Context.Set<TEntity>().Where(predicate).ToListAsync();

	}

	public async Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> predicate)
	{

		return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);

	}

	public async Task<IEnumerable<TEntity>> GetAllAsync()
	{

		return await Context.Set<TEntity>().AsNoTracking().ToListAsync();

	}

	public async Task<TEntity?> GetByIdAsync(ObjectId id)
	{

		return await Context.Set<TEntity>()
				.SingleOrDefaultAsync(a => a.Id == id);

	}
	public void Remove(TEntity entity)
	{

		Context.Set<TEntity>().Remove(entity);

	}

	public void Update(TEntity entity)
	{

		Context.Set<TEntity>().Update(entity);

	}

}
