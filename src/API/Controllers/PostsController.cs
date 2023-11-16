using Application.Dtos;
using Application.Features.Blogs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[Route("api/Posts")]
[ApiController]
[Produces("application/json")]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get list of all blogs
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<BlogDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<BlogDto>>> Get()
    {
        var query = new GetBlogsListQuery();

        return Ok(await _mediator.Send(query));
    }

    /// <summary>
    /// Get a single specific blog by it's unique id (GUID)
    /// </summary>
    /// <param name="blogId"></param>
    /// <returns></returns>
    [HttpGet("{blogId}")]
    [ProducesResponseType(typeof(BlogDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<BlogDto>> GetById(string blogId)
    {
        var query = new GetBlogByIdQuery()
        {
            BlogId = Guid.Parse(blogId)
        };

        var result = await _mediator.Send(query);

        return result == null ? NotFound() : Ok(result);
    }

}
