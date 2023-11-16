using Application.Contracts.Persistence;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Blogs.Commands;

public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand, Result<Unit>>
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


    public async Task<Result<Unit>> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var entityToUpdate = await _blogRepository.GetByIdAsync(request.BlogId);
        if (entityToUpdate == null)
        {
            _logger.LogError($"Error: Blog does not exist");
            return Result<Unit>.Failure($"Blog does not exist");
        }

        _mapper.Map(request, entityToUpdate, typeof(UpdateBlogCommand), typeof(Blog));

        await _blogRepository.UpdateAsync(entityToUpdate).ConfigureAwait(false);

        _logger.LogInformation($"Blog {entityToUpdate.Id} successfully updated");

        return Result<Unit>.Success(Unit.Value);
    }
}
