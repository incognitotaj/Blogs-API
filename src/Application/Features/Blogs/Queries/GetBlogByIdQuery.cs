using Application.Dtos;
using MediatR;

namespace Application.Features.Blogs.Queries;

public class GetBlogByIdQuery : IRequest<BlogDto>
{
    public Guid BlogId { get; set; }
}
