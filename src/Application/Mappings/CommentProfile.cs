using Application.Dtos;
using Application.Features.Comments.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>();

        CreateMap<Comment, CreateCommentCommand>()
            .ReverseMap();

        CreateMap<Comment, UpdateCommentCommand>()
            .ReverseMap();
    }
}
