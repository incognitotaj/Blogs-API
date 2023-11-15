using Domain.Common;

namespace Domain.Entities;

public class Comment : EntityBase
{
    public Comment(Guid blogId, string description)
    {
        BlogId = blogId;
        Description = description;
    }

    public Guid BlogId { get; set; }
    public virtual Blog Blog { get; set; }
    public string Description { get; set; }
}
