using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos;

public class CommentDto
{
    public Guid CommentId { get; set; }
    public string Description { get; set; }
}
