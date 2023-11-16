using Application.Contracts.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
{
    public CommentRepository(BlogContext context)
            : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetByBlogIdAsync(Guid blogId)
    {
        var list = await _context.Comments
                                .Where(p => p.BlogId == blogId)
                                .ToListAsync();

        return list;
    }
}
