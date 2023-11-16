using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.Blogs.Queries;

public class GetBlogsListQueryHandler : IRequestHandler<GetBlogsListQuery, Result<List<BlogDto>>>
{
    private readonly IMapper _mapper;
    private readonly IBlogRepository _blogRepository;

    public GetBlogsListQueryHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<BlogDto>>> Handle(GetBlogsListQuery request, CancellationToken cancellationToken)
    {
        var resultList = await _blogRepository.GetAsync(
                predicate: null, orderBy: null, includeString: "Comments", disableTracking: true).ConfigureAwait(false);

        return Result<List<BlogDto>>.Success(_mapper.Map<List<BlogDto>>(resultList));
    }
}
