using MediatR;

namespace Application.Features.Blogs.Commands;

public class UpdateBlogCommand : IRequest
{
    public Guid BlogId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
