namespace TailwindBlog.Domain.Interfaces;

public interface IUnitOfWork
{

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
