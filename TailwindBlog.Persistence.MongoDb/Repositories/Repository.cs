// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Repository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

using MongoDB.Bson;

namespace TailwindBlog.Persistence.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
	where TEntity : Entity
{
	protected readonly IMongoCollection<TEntity> Collection;

	protected Repository(IMongoDatabase database, string collectionName)
	{
		Collection = database.GetCollection<TEntity>(collectionName);
	}

	public void Add(TEntity entity)
	{
		Collection.InsertOne(entity);
	}

	public async Task AddRangeAsync(IEnumerable<TEntity> entities)
	{
		await Collection.InsertManyAsync(entities);
	}

	public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
	{
		return await Collection.Find(predicate).AnyAsync();
	}

	public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> criteria)
	{
		return await Collection.Find(criteria).ToListAsync();
	}

	public async Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> predicate)
	{
		return await Collection.Find(predicate).FirstOrDefaultAsync();
	}

	public async Task<TEntity?> GetByIdAsync(ObjectId id)
	{
		return await Collection.Find(e => e.Id == id).FirstOrDefaultAsync();
	}

	public void Remove(TEntity entity)
	{
		Collection.DeleteOne(e => e.Id == entity.Id);
	}

	public void RemoveRange(IEnumerable<TEntity> entities)
	{
		var ids = entities.Select(e => e.Id).ToList();
		Collection.DeleteMany(e => ids.Contains(e.Id));
	}

	public async Task<IEnumerable<TEntity>> GetAllAsync()
	{
		return await Collection.Find(_ => true).ToListAsync();
	}

	public void Update(TEntity entity)
	{
		Collection.ReplaceOne(e => e.Id == entity.Id, entity);
	}
}