using Application.Contracts.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
{
    public BlogRepository(BlogContext context)
            : base(context)
    {
    }
    public async Task<IEnumerable<Blog>> GetByUserAsync(Guid userId)
    {
        var list = await _context.Blogs
                                //.Where(p => p.Id == userId)
                                .ToListAsync();

        return list;
    }
}
