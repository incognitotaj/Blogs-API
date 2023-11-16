using MediatR;

namespace Application.Features.Comments.Commands
{
    public class UpdateCommentCommand : IRequest<Unit>
    {
        public Guid BlogId { get; set; }
        public Guid CommentId { get; set; }
        public string Description { get; set; }
    }
}
