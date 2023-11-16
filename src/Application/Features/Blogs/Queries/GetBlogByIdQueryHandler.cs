using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Blogs.Queries;

public class GetBlogByIdQueryHandler : IRequestHandler<GetBlogByIdQuery, Result<BlogDto>>
{
    private readonly IMapper _mapper;
    private readonly IBlogRepository _blogRepository;
    private readonly ILogger<GetBlogByIdQueryHandler> _logger;

    public GetBlogByIdQueryHandler(IBlogRepository blogRepository, IMapper mapper, ILogger<GetBlogByIdQueryHandler> logger)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<BlogDto>> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _blogRepository.GetByIdAsync(request.BlogId).ConfigureAwait(false);
        if (entity == null)
        {
            _logger.LogError($"Error: Blog does not exist");
            return Result<BlogDto>.Failure($"Error: Blog does not exist");
        }

        return Result<BlogDto>.Success(_mapper.Map<BlogDto>(entity));
    }
}
