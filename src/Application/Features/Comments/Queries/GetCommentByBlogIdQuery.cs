using Application.Dtos;
using MediatR;

namespace Application.Features.Comments.Queries;

public class GetCommentByBlogIdQuery : IRequest<IEnumerable<CommentDto>>
{
    public Guid BlogId { get; set; }
}
