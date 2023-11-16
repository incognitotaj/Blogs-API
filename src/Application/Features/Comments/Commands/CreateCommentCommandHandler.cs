using Application.Contracts.Persistence;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Comments.Commands
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
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

        public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _blogRepository.GetByIdAsync(request.BlogId);
            if (entity == null)
            {
                _logger.LogError($"Error: Blog does not exist");
                return Guid.Empty;
            }

            var comment = _mapper.Map<Comment>(request);

            var newEntity = await _commentRepository.AddAsync(comment);

            _logger.LogInformation($"Comment {newEntity.Id} created successfully on {newEntity.CreatedOn}");

            return newEntity.Id;
        }
    }
}
