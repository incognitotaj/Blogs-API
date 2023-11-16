using Application.Dtos;
using Application.Responses;
using MediatR;

namespace Application.Features.Comments.Queries;

public class GetCommentByIdQuery : IRequest<Result<CommentDto>>
{
    public Guid BlogId { get; set; }
    public Guid CommentId { get; set; }
}
