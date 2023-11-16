using Application.Contracts.Persistence;
using Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Blogs.Commands
{
    public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, Result<Unit>>
    {
        private readonly IBlogRepository _BlogRepository;
        private readonly ILogger<DeleteBlogCommandHandler> _logger;

        public DeleteBlogCommandHandler(IBlogRepository BlogRepository, ILogger<DeleteBlogCommandHandler> logger)
        {
            _BlogRepository = BlogRepository;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            var entityToDelete = await _BlogRepository.GetByIdAsync(request.BlogId);
            if (entityToDelete == null)
            {
                _logger.LogError($"Error: Blog does not exist");
                return Result<Unit>.Failure($"Blog does not exist");
            }
            
            await _BlogRepository.DeleteAsync(entityToDelete).ConfigureAwait(false);
            _logger.LogInformation($"Blog {entityToDelete.Id} successfully deleted");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
