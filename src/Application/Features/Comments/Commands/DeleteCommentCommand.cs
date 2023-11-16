using MediatR;

namespace Application.Features.Comments.Commands
{
    public class DeleteCommentCommand : IRequest
    {
        public Guid BlogId { get; set; }
        public Guid CommentId { get; set; }
    }
}
