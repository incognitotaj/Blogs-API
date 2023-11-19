using Application.Contracts.Persistence;
using Application.Contracts.Services;
using Application.Dtos;
using Application.Features.Blogs.Queries;
using Application.Mappings;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests;

[TestClass]
public class GetBlogByIdQueryHandlerTests
{
    private readonly Mock<IBlogRepository> _mockBlogRepository;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<GetBlogByIdQueryHandler>> _mockLogger;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<IConfiguration> _mockConfiguration;
    public GetBlogByIdQueryHandlerTests()
    {
        _mockBlogRepository = new Mock<IBlogRepository>();
        _mockLogger = new Mock<ILogger<GetBlogByIdQueryHandler>>();
        _mockCacheService = new Mock<ICacheService>();
        _mockConfiguration = new Mock<IConfiguration>();

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BlogProfile());
            cfg.AddProfile(new CommentProfile());
        });

        _mapper = mockMapper.CreateMapper();
    }

    [TestMethod]
    public async Task ShouldReturnsFailureWhenBlogNotFound()
    {
        // Arrange
        var query = new GetBlogByIdQuery()
        {
            BlogId = Guid.NewGuid(),
        };

        var sut = new GetBlogByIdQueryHandler(
            _mockBlogRepository.Object,
            _mapper,
            _mockLogger.Object,
            _mockCacheService.Object,
            _mockConfiguration.Object);

        var configurationSectionMockUrl = new Mock<IConfigurationSection>();
        var configurationSectionMockEnabled = new Mock<IConfigurationSection>();

        configurationSectionMockUrl
           .Setup(x => x.Value)
           .Returns("");

        configurationSectionMockEnabled
           .Setup(x => x.Value)
           .Returns("false");

        _mockConfiguration
           .Setup(x => x.GetSection("Redis:Url"))
           .Returns(configurationSectionMockUrl.Object);

        _mockConfiguration
           .Setup(x => x.GetSection("Redis:IsEnabled"))
           .Returns(configurationSectionMockEnabled.Object);


        _mockBlogRepository.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync((Blog)null);

        // Act
        Result<BlogDto> result = await sut.Handle(query, default);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNull(result.Value);
        Assert.AreEqual(result.Error, "Error: Blog does not exist");
    }

    [TestMethod]
    public async Task ShouldReturnsSuccessWhenBlogFound()
    {
        // Arrange
        var query = new GetBlogByIdQuery()
        {
            BlogId = Guid.NewGuid(),
        };

        var sut = new GetBlogByIdQueryHandler(
            _mockBlogRepository.Object,
            _mapper,
            _mockLogger.Object,
            _mockCacheService.Object,
            _mockConfiguration.Object);

        var configurationSectionMockUrl = new Mock<IConfigurationSection>();
        var configurationSectionMockEnabled = new Mock<IConfigurationSection>();

        configurationSectionMockUrl
           .Setup(x => x.Value)
           .Returns("");

        configurationSectionMockEnabled
           .Setup(x => x.Value)
           .Returns("false");

        _mockConfiguration
           .Setup(x => x.GetSection("Redis:Url"))
           .Returns(configurationSectionMockUrl.Object);

        _mockConfiguration
           .Setup(x => x.GetSection("Redis:IsEnabled"))
           .Returns(configurationSectionMockEnabled.Object);

        var blog = new Blog(title: "First blog", description: "Sample blog # 1")
        {
            CreatedOn = DateTime.Now,
            UpdatedOn = null,
            Id = Guid.NewGuid(),
        };

        _mockBlogRepository.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(blog);

        // Act
        Result<BlogDto> result = await sut.Handle(query, default);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNull(result.Error);
    }
}