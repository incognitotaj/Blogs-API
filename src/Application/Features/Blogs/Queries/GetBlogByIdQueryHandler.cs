using Application.Contracts.Persistence;
using Application.Contracts.Services;
using Application.Dtos;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Features.Blogs.Queries;

public class GetBlogByIdQueryHandler : IRequestHandler<GetBlogByIdQuery, Result<BlogDto>>
{
    private readonly IMapper _mapper;
    private readonly IBlogRepository _blogRepository;
    private readonly ILogger<GetBlogByIdQueryHandler> _logger;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;

    public GetBlogByIdQueryHandler(IBlogRepository blogRepository,
        IMapper mapper,
        ILogger<GetBlogByIdQueryHandler> logger,
        ICacheService cacheService,
        IConfiguration configuration)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
        _logger = logger;
        _cacheService = cacheService;
        _configuration = configuration;
    }

    public async Task<Result<BlogDto>> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        if (_configuration.GetValue<bool>("Redis:IsEnabled"))
        {
            var cachedData = _cacheService.GetData<BlogDto>($"blog-{request.BlogId}");
            if (cachedData != null)
            {
                return Result<BlogDto>.Success(cachedData);
            }
        }
        var entity = await _blogRepository.GetByIdAsync(request.BlogId).ConfigureAwait(false);
        if (entity == null)
        {
            _logger.LogError($"Error: Blog does not exist");
            return Result<BlogDto>.Failure($"Error: Blog does not exist");
        }

        var dto = _mapper.Map<BlogDto>(entity);
        
        if (_configuration.GetValue<bool>("Redis:IsEnabled"))
            _cacheService.SetData($"blog-{request.BlogId}", dto, DateTimeOffset.Now.AddMinutes(15));
        
        return Result<BlogDto>.Success(dto);
    }
}
