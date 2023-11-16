namespace Application.Dtos;

public class BlogDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<CommentDto> Comments { get; set; }

    public BlogDto()
    {
        Comments = new List<CommentDto>();
    }
}
