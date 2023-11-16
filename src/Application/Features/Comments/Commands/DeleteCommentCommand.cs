using Application.Responses;
using MediatR;

namespace Application.Features.Comments.Commands
{
    public class DeleteCommentCommand : IRequest<Result<Unit>>
    {
        public Guid BlogId { get; set; }
        public Guid CommentId { get; set; }
    }
}
