using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Comments.Queries;

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, Result<CommentDto>>
{
    private readonly IMapper _mapper;
    private readonly IBlogRepository _blogRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<GetCommentByIdQueryHandler> _logger;

    public GetCommentByIdQueryHandler(
        IBlogRepository blogRepository,
        ICommentRepository commentRepository,
        IMapper mapper,
        ILogger<GetCommentByIdQueryHandler> logger)
    {
        _blogRepository = blogRepository;
        _commentRepository = commentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<CommentDto>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
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

        return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
    }
}
