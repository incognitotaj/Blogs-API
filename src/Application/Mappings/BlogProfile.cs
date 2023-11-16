using Application.Dtos;
using Application.Features.Blogs.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<Blog, BlogDto>();
        CreateMap<Blog, CreateBlogCommand>().ReverseMap();
        CreateMap<Blog, UpdateBlogCommand>().ReverseMap();

    }
}
