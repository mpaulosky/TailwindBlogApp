// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Repository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =======================================================

namespace Persistence.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
	where TEntity : Entity
{
	protected readonly IMongoCollection<TEntity> Collection;

	protected Repository(IMongoDbContextFactory context)
	{

		ArgumentNullException.ThrowIfNull(context);

		var collectionName = CollectionNames.GetCollectionName(nameof(TEntity));

		Collection = context.GetCollection<TEntity>(collectionName);
		
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

			// Archive the entity
			entity.Archived = true;

			var filter = Builders<TEntity>.Filter.Eq("_id", entity.Id);

			var result = await Collection.ReplaceOneAsync(filter, entity);

			return result.MatchedCount == 0 ? Result.Fail($"TEntity with ID {entity.Id} not found") : Result.Ok();

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

			await Collection.InsertOneAsync(entity);

			return Result.Ok();

		}
		catch (Exception ex)
		{

			return Result.Fail(ex.Message);

		}

	}

	/// <summary>
	///   Retrieves an entity from the database by its ID
	/// </summary>
	/// <param name="entityId">ObjectId of the entity to retrieve</param>
	/// <returns>Result containing the retrieved TEntity or error message</returns>
	public async Task<Result<TEntity>> GetAsync(ObjectId entityId)
	{

		try
		{

			var filter = Builders<TEntity>.Filter.Eq("_id", entityId);

			var result = (await Collection.FindAsync(filter)).ToList().FirstOrDefault();

			return result == null
					? Result<TEntity>.Fail($"TEntity with ID {entityId} not found")
					: Result<TEntity>.Ok(result.Adapt<TEntity>());

		}
		catch (Exception ex)
		{

			return Result<TEntity>.Fail(ex.Message);

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

			var filter = Builders<TEntity>.Filter.Empty;

			var result = (await Collection.FindAsync(filter)).ToList();

			return Result<IEnumerable<TEntity>>.Ok(result.Adapt<IEnumerable<TEntity>>());

		}
		catch (Exception ex)
		{

			return Result<IEnumerable<TEntity>>.Fail(ex.Message);

		}

	}

	/// <summary>
	///   Updates an existing entity in the database
	/// </summary>
	/// <param name="entityId">ObjectId of the entity to update</param>
	/// <param name="entity">TEntity containing the updated entity data</param>
	/// <returns>Result indicating success or failure of the operation</returns>
	public async Task<Result> UpdateAsync(ObjectId entityId, TEntity entity)
	{

		try
		{

			var entityToUpdate = entity.Adapt<TEntity>();

			var filter = Builders<TEntity>.Filter.Eq("_id", entityId);

			var result = await Collection.ReplaceOneAsync(filter, entityToUpdate);

			return result.MatchedCount == 0 ? Result.Fail($"TEntity with ID {entityId} not found") : Result.Ok();

		}
		catch (Exception ex)
		{

			return Result.Fail(ex.Message);

		}

	}

}