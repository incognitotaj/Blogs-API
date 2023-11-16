using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Blogs.Commands;

public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateBlogCommandHandler> _logger;

    public UpdateBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper, ILogger<UpdateBlogCommandHandler> logger)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<Unit> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var entityToUpdate = await _blogRepository.GetByIdAsync(request.BlogId);
        if (entityToUpdate == null)
        {
            _logger.LogError($"Error: Project does not exist");
        }

        _mapper.Map(request, entityToUpdate, typeof(UpdateBlogCommand), typeof(Blog));

        await _blogRepository.UpdateAsync(entityToUpdate).ConfigureAwait(false);

        _logger.LogInformation($"Blog {entityToUpdate.Id} successfully updated");

        return Unit.Value;
    }
}
