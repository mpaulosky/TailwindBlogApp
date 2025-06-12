namespace Persistence;

/// <summary>
/// Defines the contract for database context access
/// </summary>
public interface IApplicationDbContext
{
	/// <summary>
	/// Gets the collection of articles
	/// </summary>
	IMongoCollection<Article> Articles { get; }

	/// <summary>
	/// Gets the collection of categories
	/// </summary>
	IMongoCollection<Category> Categories { get; }
}
