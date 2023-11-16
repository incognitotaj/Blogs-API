using Application.Contracts.Persistence;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Blogs.Commands;

public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, Result<Guid>>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateBlogCommandHandler> _logger;

    public CreateBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper, ILogger<CreateBlogCommandHandler> logger)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<Result<Guid>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Blog>(request);

        var newEntity = await _blogRepository.AddAsync(entity);
        if (newEntity != null)
        {
            _logger.LogInformation($"Blog {newEntity.Id} created successfully on {newEntity.CreatedOn}");
            return Result<Guid>.Success(newEntity.Id);
        }

        return Result<Guid>.Failure($"Fail to create the Blog");
    }
}
