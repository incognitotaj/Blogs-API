using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/Posts/{postId}/Comments")]
[ApiController]
public class CommentsController : ControllerBase
{
    public CommentsController()
    {
    }
}
