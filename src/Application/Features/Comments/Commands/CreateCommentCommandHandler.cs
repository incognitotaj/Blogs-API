using Application.Contracts.Persistence;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Comments.Commands
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<Guid>>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCommentCommandHandler> _logger;

        public CreateCommentCommandHandler(
            IBlogRepository blogRepository,
            ICommentRepository commentRepository,
            IMapper mapper,
            ILogger<CreateCommentCommandHandler> logger)
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _blogRepository.GetByIdAsync(request.BlogId);
            if (entity == null)
            {
                _logger.LogError($"Error: Blog does not exist");
                return Result<Guid>.Failure($"Blog does not exist");
            }

            var comment = _mapper.Map<Comment>(request);

            var newEntity = await _commentRepository.AddAsync(comment);
            if (newEntity == null)
            {
                return Result<Guid>.Failure("Error creating the comment");
            }

            _logger.LogInformation($"Comment {newEntity.Id} created successfully on {newEntity.CreatedOn}");
            return Result<Guid>.Success(newEntity.Id);

        }
    }
}
