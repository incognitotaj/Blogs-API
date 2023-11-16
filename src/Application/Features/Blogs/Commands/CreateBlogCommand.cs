using MediatR;

namespace Application.Features.Blogs.Commands;

public class CreateBlogCommand : IRequest<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
}
