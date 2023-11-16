using MediatR;

namespace Application.Features.Blogs.Commands;

public class DeleteBlogCommand : IRequest
{
    public Guid BlogId { get; set; }
}
