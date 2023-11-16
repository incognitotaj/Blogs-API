namespace API.Requests;

public class UpdateCommentRequest
{
    public Guid CommentId { get; set; }
    public Guid BlogId { get; set; }
    public string Description { get; set; }
}
