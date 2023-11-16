using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface ICommentRepository : IAsyncRepository<Comment>
{
    Task<IEnumerable<Comment>> GetByBlogIdAsync(Guid blogId);
}
