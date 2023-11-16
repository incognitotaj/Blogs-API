using Application.Dtos;
using Application.Responses;
using MediatR;

namespace Application.Features.Blogs.Queries;

public class GetBlogsListQuery : IRequest<Result<List<BlogDto>>>
{
    public GetBlogsListQuery()
    {
    }
}
