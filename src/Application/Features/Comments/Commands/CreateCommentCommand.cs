using MediatR;

namespace Application.Features.Comments.Commands
{
    public class CreateCommentCommand : IRequest<Guid>
    {
        public Guid BlogId { get; set; }
        public string Description { get; set; }
    }
}
