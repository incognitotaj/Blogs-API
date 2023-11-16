using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Comments.Queries;

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentDto>
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

    public async Task<CommentDto> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        Comment? comment = null;

        var entityMain = await _blogRepository.GetByIdAsync(request.BlogId).ConfigureAwait(false);
        if (entityMain == null)
        {
            _logger.LogError($"Error: Blog does not exist");
        }
        else
        {
            comment = await _commentRepository.GetByIdAsync(request.CommentId).ConfigureAwait(false);
            if (comment == null)
            {
                _logger.LogError($"Error: Comment does not exist");
            }
        }

        return _mapper.Map<CommentDto>(comment);
    }
}
