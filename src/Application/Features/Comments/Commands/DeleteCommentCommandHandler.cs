using Application.Contracts.Persistence;
using Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Comments.Commands
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result<Unit>>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCommentCommandHandler> _logger;

        public DeleteCommentCommandHandler(
            IBlogRepository blogRepository,
            ICommentRepository commentRepository,
            IMapper mapper,
            ILogger<DeleteCommentCommandHandler> logger)
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var entityMain = await _blogRepository.GetByIdAsync(request.BlogId);
            if (entityMain == null)
            {
                _logger.LogError($"Error: Blog does not exist");
                return Result<Unit>.Failure($"Blog does not exist");
            }
            var entityToDelete = await _commentRepository.GetByIdAsync(request.CommentId);
            if (entityToDelete == null)
            {
                _logger.LogError($"Error: Comment does not exist");
                return Result<Unit>.Failure($"Comment does not exist");
            }

            await _commentRepository.DeleteAsync(entityToDelete).ConfigureAwait(false);
            _logger.LogInformation($"Comment {entityToDelete.Id} successfully deleted");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
