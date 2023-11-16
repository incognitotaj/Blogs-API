using Application.Dtos;
using MediatR;

namespace Application.Features.Blogs.Queries;

public class GetBlogsListQuery : IRequest<List<BlogDto>>
{
    public GetBlogsListQuery()
    {
    }
}
