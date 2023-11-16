using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<Blog, BlogDto>();
    }
}
