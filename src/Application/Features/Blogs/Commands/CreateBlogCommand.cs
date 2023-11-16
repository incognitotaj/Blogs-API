using Application.Responses;
using FluentValidation;
using MediatR;

namespace Application.Features.Blogs.Commands;

public class CreateBlogCommand : IRequest<Result<Guid>>
{
    public string Title { get; set; }
    public string Description { get; set; }
}

public class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(p => p.Title).NotEmpty();
        RuleFor(p => p.Description).NotEmpty();
    }
}
