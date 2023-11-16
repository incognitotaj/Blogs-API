namespace API.Requests;

public class UpdateBlogRequest
{
    public Guid BlogId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
