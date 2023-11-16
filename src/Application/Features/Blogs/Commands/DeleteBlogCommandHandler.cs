using Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Blogs.Commands
{
    public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand>
    {
        private readonly IBlogRepository _BlogRepository;
        private readonly ILogger<DeleteBlogCommandHandler> _logger;

        public DeleteBlogCommandHandler(IBlogRepository BlogRepository, ILogger<DeleteBlogCommandHandler> logger)
        {
            _BlogRepository = BlogRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            var entityToDelete = await _BlogRepository.GetByIdAsync(request.BlogId);
            if (entityToDelete == null)
            {
                _logger.LogError($"Error: Blog does not exist");
            }
            else
            {
                await _BlogRepository.DeleteAsync(entityToDelete).ConfigureAwait(false);
                _logger.LogInformation($"Blog {entityToDelete.Id} successfully deleted");
            }
            return Unit.Value;
        }
    }
}
