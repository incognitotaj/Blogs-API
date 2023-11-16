using Application.Contracts.Persistence;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Comments.Queries;

public class GetCommentByBlogIdQueryHandler : IRequestHandler<GetCommentByBlogIdQuery, IEnumerable<CommentDto>>
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

    public async Task<IEnumerable<CommentDto>> Handle(GetCommentByBlogIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Comment>? comments = null;
        var entity = await _blogRepository.GetByIdAsync(request.BlogId).ConfigureAwait(false);
        if (entity == null)
        {
            _logger.LogError($"Error: Blog does not exist");
        }
        else
        {
            comments = await _commentRepository.GetByBlogIdAsync(entity.Id).ConfigureAwait(false);
        }

        return _mapper.Map<IEnumerable<CommentDto>>(comments);
    }
}
