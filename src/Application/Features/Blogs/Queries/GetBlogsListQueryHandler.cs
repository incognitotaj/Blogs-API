using Application.Contracts.Persistence;
using Application.Contracts.Services;
using Application.Dtos;
using Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Blogs.Queries;

public class GetBlogsListQueryHandler : IRequestHandler<GetBlogsListQuery, Result<List<BlogDto>>>
{
    private readonly IMapper _mapper;
    private readonly IBlogRepository _blogRepository;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;

    public GetBlogsListQueryHandler(
        IBlogRepository blogRepository, 
        IMapper mapper,
        ICacheService cacheService,
        IConfiguration configuration)

    {
        _blogRepository = blogRepository;
        _mapper = mapper;
        _cacheService = cacheService;
        _configuration = configuration;
    }

    public async Task<Result<List<BlogDto>>> Handle(GetBlogsListQuery request, CancellationToken cancellationToken)
    {
        if (_configuration.GetValue<bool>("Redis:IsEnabled"))
        {
            var cachedData = _cacheService.GetData<List<BlogDto>>($"blogs");
            if (cachedData != null)
            {
                return Result<List<BlogDto>>.Success(cachedData);
            }
        }
        var resultList = await _blogRepository.GetAsync(
                predicate: null, orderBy: null, includeString: "Comments", disableTracking: true).ConfigureAwait(false);


        var result = _mapper.Map<List<BlogDto>>(resultList);
        
        if (_configuration.GetValue<bool>("Redis:IsEnabled"))
            _cacheService.SetData($"blogs", result, DateTimeOffset.Now.AddMinutes(15));

        return Result<List<BlogDto>>.Success(result);
    }
}
