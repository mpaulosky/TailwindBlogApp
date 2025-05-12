namespace TailwindBlog.Domain.Interfaces;

public interface IApplicationDbContext
{

	DbSet<Article> Articles { get; set; }

	DbSet<Category> Categories { get; set; }

}
