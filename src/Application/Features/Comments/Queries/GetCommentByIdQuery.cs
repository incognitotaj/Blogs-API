using Application.Dtos;
using MediatR;

namespace Application.Features.Comments.Queries;

public class GetCommentByIdQuery : IRequest<CommentDto>
{
    public Guid BlogId { get; set; }
    public Guid CommentId { get; set; }
}
