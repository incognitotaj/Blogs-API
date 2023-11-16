using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface IBlogRepository : IAsyncRepository<Blog>
{
    // Get Blogs By User
    Task<IEnumerable<Blog>> GetByUserAsync(Guid userId);
}
