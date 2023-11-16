using API.Requests;
using Application.Dtos;
using Application.Features.Comments.Commands;
using Application.Features.Comments.Queries;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[Route("api/Posts/{blogId}/Comments")]
[ApiController]
[Produces("application/json")]
//[Authorize]
public class CommentsController : BaseApiController
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns Collection of Comments for the specific Blog
    /// </summary>
    /// <param name="blogId"></param>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpGet("{commentId}")]
    [ProducesResponseType(typeof(Result<CommentDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Get(string blogId, string commentId)
    {
        var query = new GetCommentByIdQuery()
        {
            BlogId = Guid.Parse(blogId),
            CommentId = Guid.Parse(commentId)
        };

        var result = await _mediator.Send(query);

        return HandleResult(result);
    }

    /// <summary>
    /// Get list of all comments for the specific blog
    /// </summary>
    /// <param name="blogId"></param>
    /// <returns></returns>
    [HttpGet()]
    [ProducesResponseType(typeof(Result<IEnumerable<CommentDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetByBlog(string blogId)
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
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Create(string blogId, [FromBody] CreateCommentRequest request)
    {
        var command = new CreateCommentCommand
        {
            BlogId = Guid.Parse(blogId),
            Description = request.Description,
        };

        var result = await _mediator.Send(command);
        return HandleResult(result);
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
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Update(string blogId, [FromBody] UpdateCommentRequest request)
    {
        var command = new UpdateCommentCommand
        {
            BlogId = Guid.Parse(blogId),
            CommentId = request.CommentId,
        };
        var result = await _mediator.Send(command);
        return HandleResult(result);
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
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Delete(string blogId, string commentId)
    {
        var command = new DeleteCommentCommand()
        {
            BlogId = Guid.Parse(blogId),
            CommentId = Guid.Parse(commentId)
        };
        var result = await _mediator.Send(command);
        return HandleResult(result);
    }
}
