using Application.Dtos;
using Application.Responses;
using MediatR;

namespace Application.Features.Blogs.Queries;

public class GetBlogByIdQuery : IRequest<Result<BlogDto>>
{
    public Guid BlogId { get; set; }
}
