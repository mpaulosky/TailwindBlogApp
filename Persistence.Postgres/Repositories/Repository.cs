// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Repository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =======================================================

namespace Persistence.Postgres.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
		where TEntity : Entity
{

	private readonly DbSet<TEntity> _dbSet;

	protected readonly PgContext Context;

	protected Repository(PgContext? context)
	{
		ArgumentNullException.ThrowIfNull(context);
		Context = context;
		_dbSet = context.Set<TEntity>();
	}

	/// <summary>
	///   Archives an entity by setting its Archived property to true and updating it
	/// </summary>
	/// <param name="entity">The TEntity to archive</param>
	/// <returns>Result indicating success or failure of the operation</returns>
	public async Task<Result> ArchiveAsync(TEntity entity)
	{
		try
		{
			if (entity is null)
			{
				return Result.Fail("Entity is null");
			}

			// Archive the entity
			entity.Archived = true;
			entity.ModifiedOn = DateTime.UtcNow;

			_dbSet.Update(entity);
			await Context.SaveChangesAsync();

			return Result.Ok();
		}
		catch (Exception ex)
		{
			return Result.Fail(ex.Message);
		}
	}

	/// <summary>
	///   Creates a new entity in the database
	/// </summary>
	/// <param name="entity">TEntity to create</param>
	/// <returns>Result indicating success or failure of the operation</returns>
	public async Task<Result> CreateAsync(TEntity entity)
	{

		try
		{

			await _dbSet.AddAsync(entity);
			await Context.SaveChangesAsync();

			return Result.Ok();

		}
		catch (Exception ex)
		{

			return Result.Fail(ex.Message);

		}

	}

	/// <summary>
	///   Retrieves all entities from the database
	/// </summary>
	/// <returns>Result containing a collection of TEntity objects or error message</returns>
	public async Task<Result<IEnumerable<TEntity>>> GetAllAsync()
	{

		try
		{

			var result = await _dbSet.ToListAsync();

			return result == null || !result.Any()
					? Result<IEnumerable<TEntity>>.Fail("No entities found")
					: Result<IEnumerable<TEntity>>.Ok(result.Adapt<IEnumerable<TEntity>>());

		}
		catch (Exception ex)
		{

			return Result<IEnumerable<TEntity>>.Fail(ex.Message);

		}

	}

	/// <summary>
	///   Retrieves an entity from the database by its ID
	/// </summary>
	/// <param name="entityId">Guid of the entity to retrieve</param>
	/// <returns>Result containing the retrieved TEntity or error message</returns>
	public async Task<Result<TEntity>> GetAsync(Guid entityId)
	{

		try
		{

			var result = await _dbSet.FindAsync(entityId);

			return result == null
					? Result<TEntity>.Fail($"TEntity with ID {entityId} not found")
					: Result<TEntity>.Ok(result);

		}
		catch (Exception ex)
		{

			return Result<TEntity>.Fail(ex.Message);

		}

	}

	/// <summary>
	///   Updates an existing entity in the database
	/// </summary>
	/// <param name="entityId">ObjectId of the entity to update</param>
	/// <param name="entity">TEntity containing the updated entity data</param>
	/// <returns>Result indicating success or failure of the operation</returns>
	public async Task<Result> UpdateAsync(Guid entityId, TEntity entity)
	{

		try
		{

			var entityToUpdate = entity.Adapt<TEntity>();

			Context.Entry(entityToUpdate).State = EntityState.Modified;

			await Context.SaveChangesAsync();

			return Result.Ok();

		}
		catch (Exception ex)
		{

			return Result.Fail(ex.Message);

		}

	}

}