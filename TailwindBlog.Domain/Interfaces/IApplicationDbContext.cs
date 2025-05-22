
namespace TailwindBlog.Domain.Interfaces;

public interface IApplicationDbContext
{
	IMongoCollection<Article> Articles { get; }
	IMongoCollection<Category> Categories { get; }
}