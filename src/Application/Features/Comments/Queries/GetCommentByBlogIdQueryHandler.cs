using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Comments.Queries;

public class GetCommentByBlogIdQueryHandler : IRequestHandler<GetCommentByBlogIdQuery, Result<IEnumerable<CommentDto>>>
{
    private readonly IBlogRepository _blogRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCommentByBlogIdQueryHandler> _logger;

    public GetCommentByBlogIdQueryHandler(
        IBlogRepository blogRepository,
        ICommentRepository commentRepository,
        IMapper mapper,
        ILogger<GetCommentByBlogIdQueryHandler> logger)
    {
        _blogRepository = blogRepository;
        _commentRepository = commentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<CommentDto>>> Handle(GetCommentByBlogIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _blogRepository.GetByIdAsync(request.BlogId).ConfigureAwait(false);
        if (entity == null)
        {
            _logger.LogError($"Error: Blog does not exist");
            return Result<IEnumerable<CommentDto>>.Failure($"Blog does not exist");
        }
        
        var comments = await _commentRepository.GetByBlogIdAsync(entity.Id).ConfigureAwait(false);

        return Result<IEnumerable<CommentDto>>.Success(_mapper.Map<IEnumerable<CommentDto>>(comments));
    }
}
