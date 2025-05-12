namespace TailwindBlog.Domain.Interfaces;

/// <summary>
/// Base interface for repository pattern implementation
/// </summary>
/// <typeparam name="T">The type of entity</typeparam>
public interface IRepository<T> where T : class
{
	/// <summary>
	/// Retrieves an entity by its identifier
	/// </summary>
	/// <param name="id">The identifier</param>
	/// <returns>The entity or null if not found</returns>
	Task<T?> GetByIdAsync(object id);

	/// <summary>
	/// Retrieves entities matching specified criteria
	/// </summary>
	/// <param name="criteria">The search criteria</param>
	/// <returns>Matching entities</returns>
	Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria);

	/// <summary>
	/// Adds a new entity
	/// </summary>
	/// <param name="entity">The entity to add</param>
	void Add(T entity);

	/// <summary>
	/// Updates an existing entity
	/// </summary>
	/// <param name="entity">The entity to update</param>
	void Update(T entity);

	/// <summary>
	/// Removes an entity
	/// </summary>
	/// <param name="entity">The entity to remove</param>
	void Remove(T entity);
}
