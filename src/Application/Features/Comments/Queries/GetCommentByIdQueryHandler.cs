using Application.Contracts.Persistence;
using Application.Contracts.Services;
using Application.Dtos;
using Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Features.Comments.Queries;

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, Result<CommentDto>>
{
    private readonly IMapper _mapper;
    private readonly IBlogRepository _blogRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<GetCommentByIdQueryHandler> _logger;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;

    public GetCommentByIdQueryHandler(
        IBlogRepository blogRepository,
        ICommentRepository commentRepository,
        IMapper mapper,
        ILogger<GetCommentByIdQueryHandler> logger,
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

    public async Task<Result<CommentDto>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        if (_configuration.GetValue<bool>("Redis:IsEnabled"))
        {
            var cachedData = _cacheService.GetData<CommentDto>($"comment-{request.BlogId}");
            if (cachedData != null)
            {
                return Result<CommentDto>.Success(cachedData);
            }
        }

        var entityMain = await _blogRepository.GetByIdAsync(request.BlogId).ConfigureAwait(false);
        if (entityMain == null)
        {
            _logger.LogError($"Error: Blog does not exist");
            return Result<CommentDto>.Failure($"Blog does not exist");
        }
        var comment = await _commentRepository.GetByIdAsync(request.CommentId).ConfigureAwait(false);
        if (comment == null)
        {
            _logger.LogError($"Error: Comment does not exist");
            return Result<CommentDto>.Failure($"Comment does not exist");
        }

        var result = _mapper.Map<CommentDto>(comment);
        
        if (_configuration.GetValue<bool>("Redis:IsEnabled"))
            _cacheService.SetData($"comment-{request.BlogId}", result, DateTimeOffset.Now.AddMinutes(15));
        
        return Result<CommentDto>.Success(result);
    }
}
