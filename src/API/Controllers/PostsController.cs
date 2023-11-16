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
public class PostsController : BaseApiController
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
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Get()
    {
        var query = new GetBlogsListQuery();

        var result = await _mediator.Send(query);

        return HandleResult(result);
    }

    /// <summary>
    /// Get a single specific blog by it's unique id (GUID)
    /// </summary>
    /// <param name="blogId"></param>
    /// <returns></returns>
    [HttpGet("{blogId}")]
    [ProducesResponseType(typeof(BlogDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetById(string blogId)
    {
        var query = new GetBlogByIdQuery()
        {
            BlogId = Guid.Parse(blogId)
        };

        var result = await _mediator.Send(query);

        return HandleResult(result);
    }

    /// <summary>
    /// Creates / register a new blog
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost()]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateBlogRequest request)
    {
        var command = new CreateBlogCommand()
        {
            Description = request.Description,
            Title = request.Title,
        };

        var result = await _mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Updates an existing blog
    /// </summary>
    /// <param name="blogId">Blog id of the existing record</param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{blogId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult> Update(string blogId, [FromBody] UpdateBlogRequest request)
    {
        var command = new UpdateBlogCommand()
        {
            BlogId = Guid.Parse(blogId),
            Description = request.Description,
            Title = request.Title,
        };

        var result = await _mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Deletes an existing blog
    /// </summary>
    /// <param name="blogId"></param>
    /// <returns></returns>
    [HttpDelete("{blogId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult> Delete(string blogId)
    {
        var command = new DeleteBlogCommand()
        {
            BlogId = Guid.Parse(blogId)
        };
        var result = await _mediator.Send(command);
        return HandleResult(result);
    }
}
