using Domain.Common;

namespace Domain.Entities;

public class Blog : EntityBase
{
    public Blog(string title, string description)
    {
        Title = title;
        Description = description;
        Comments = new HashSet<Comment>();
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
