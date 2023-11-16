using API.Requests;
using Application.Dtos;
using Application.Features.Comments.Commands;
using Application.Features.Comments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[Route("api/Posts/{postId}/Comments")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get specific comments by ID
    /// </summary>
    /// <returns></returns>
    [HttpGet("{commentId}")]
    [ProducesResponseType(typeof(IEnumerable<CommentDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<CommentDto>>> Get(string blogId, string commentId)
    {
        var query = new GetCommentByIdQuery()
        {
            BlogId = Guid.Parse(blogId),
            CommentId = Guid.Parse(commentId)
        };

        var result = await _mediator.Send(query);

        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Get list of all comments for the specific blog
    /// </summary>
    /// <param name="blogId"></param>
    /// <returns></returns>
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<CommentDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetByProject(string blogId)
    {
        var query = new GetCommentByBlogIdQuery
        {
            BlogId = Guid.Parse(blogId)
        };

        return Ok(await _mediator.Send(query));
    }

    /// <summary>
    /// Creates a new comment for the blog
    /// </summary>
    /// <param name="blogId">Blog Id</param>
    /// <param name="request">Comment</param>
    /// <returns></returns>
    [HttpPost()]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<Guid>> Create(string blogId, [FromBody] CreateCommentRequest request)
    {
        var command = new CreateCommentCommand
        {
            BlogId = Guid.Parse(blogId),
            Description = request.Description,
        };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing blog
    /// </summary>
    /// <param name="blogId">Blog Id</param>
    /// <param name="request">Comment</param>
    /// <returns></returns>
    [HttpPut()]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Update(string blogId, [FromBody] UpdateCommentRequest request)
    {
        var command = new UpdateCommentCommand
        {
            BlogId = Guid.Parse(blogId),
            CommentId = request.CommentId,
        };
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing comment
    /// </summary>
    /// <param name="blogId">Blog Id</param>
    /// <param name="commentId">Comment Id</param>
    /// <returns></returns>
    [HttpDelete("{commentId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(string blogId, string commentId)
    {
        var command = new DeleteCommentCommand()
        {
            BlogId = Guid.Parse(blogId),
            CommentId = Guid.Parse(commentId)
        };
        await _mediator.Send(command);
        return NoContent();
    }
}
