using Application.Contracts.Persistence;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Comments.Commands
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Result<Unit>>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommentCommandHandler> _logger;

        public UpdateCommentCommandHandler(
            IBlogRepository blogRepository,
            ICommentRepository commentRepository,
            IMapper mapper,
            ILogger<UpdateCommentCommandHandler> logger)
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var entityMain = await _blogRepository.GetByIdAsync(request.BlogId);
            if (entityMain == null)
            {
                _logger.LogError($"Error: Blog does not exist");
                return Result<Unit>.Failure($"Blog does not exist");
            }

            var entityToUpdate = await _commentRepository.GetByIdAsync(request.CommentId);
            if (entityToUpdate == null)
            {
                _logger.LogError($"Error: Comment does not exist");
                return Result<Unit>.Failure($"Comment does not exist");
            }

            _mapper.Map(request, entityToUpdate, typeof(UpdateCommentCommand), typeof(Comment));

            await _commentRepository.UpdateAsync(entityToUpdate).ConfigureAwait(false);

            _logger.LogInformation($"Comment {entityToUpdate.Id} successfully updated");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
