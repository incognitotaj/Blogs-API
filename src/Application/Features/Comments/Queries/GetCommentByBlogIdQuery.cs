using Application.Dtos;
using Application.Responses;
using MediatR;

namespace Application.Features.Comments.Queries;

public class GetCommentByBlogIdQuery : IRequest<Result<IEnumerable<CommentDto>>>
{
    public Guid BlogId { get; set; }
}
