using Application.Contracts.Persistence;
using Application.Contracts.Services;
using Application.Dtos;
using Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Features.Comments.Queries;

public class GetCommentByBlogIdQueryHandler : IRequestHandler<GetCommentByBlogIdQuery, Result<IEnumerable<CommentDto>>>
{
    private readonly IBlogRepository _blogRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCommentByBlogIdQueryHandler> _logger;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;

    public GetCommentByBlogIdQueryHandler(
        IBlogRepository blogRepository,
        ICommentRepository commentRepository,
        IMapper mapper,
        ILogger<GetCommentByBlogIdQueryHandler> logger,
        ICacheService cacheService,
        IConfiguration configuration)
    {
        _blogRepository = blogRepository;
        _commentRepository = commentRepository;
        _mapper = mapper;
        _logger = logger;
        _cacheService = cacheService;
        _configuration = configuration;
    }

    public async Task<Result<IEnumerable<CommentDto>>> Handle(GetCommentByBlogIdQuery request, CancellationToken cancellationToken)
    {
        if (_configuration.GetValue<bool>("Redis:IsEnabled"))
        {
            var cachedData = _cacheService.GetData<IEnumerable<CommentDto>>($"comments-{request.BlogId}");
            if (cachedData != null)
            {
                return Result<IEnumerable<CommentDto>>.Success(cachedData);
            }
        }
        var entity = await _blogRepository.GetByIdAsync(request.BlogId).ConfigureAwait(false);
        if (entity == null)
        {
            _logger.LogError($"Error: Blog does not exist");
            return Result<IEnumerable<CommentDto>>.Failure($"Blog does not exist");
        }

        var comments = await _commentRepository.GetByBlogIdAsync(entity.Id).ConfigureAwait(false);

        var result = _mapper.Map<IEnumerable<CommentDto>>(comments);

        if (_configuration.GetValue<bool>("Redis:IsEnabled"))
            _cacheService.SetData($"comments-{request.BlogId}", result, DateTimeOffset.Now.AddMinutes(15));

        return Result<IEnumerable<CommentDto>>.Success(result);
    }
}
