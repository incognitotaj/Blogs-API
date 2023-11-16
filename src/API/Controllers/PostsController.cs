using API.Requests;
using Application.Dtos;
using Application.Features.Blogs.Commands;
using Application.Features.Blogs.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[Route("api/Posts")]
[ApiController]
[Produces("application/json")]
[Authorize]
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

    /// <summary>
    /// Creates / register a new blog
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost()]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateBlogRequest request)
    {
        var command = new CreateBlogCommand()
        {
            Description = request.Description,
            Title = request.Title,
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing blog
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{blogId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Update(string blogId, [FromBody] UpdateBlogRequest request)
    {
        var command = new UpdateBlogCommand()
        {
            BlogId = Guid.Parse(blogId),
            Description = request.Description,
            Title = request.Title,
        };

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing blog
    /// </summary>
    /// <param name="blogId"></param>
    /// <returns></returns>
    [HttpDelete("{blogId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(string blogId)
    {
        var command = new DeleteBlogCommand()
        {
            BlogId = Guid.Parse(blogId)
        };
        await _mediator.Send(command);
        return NoContent();
    }
}
