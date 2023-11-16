using Application.Responses;
using FluentValidation;
using MediatR;

namespace Application.Features.Blogs.Commands;

public class UpdateBlogCommand : IRequest<Result<Unit>>
{
    public Guid BlogId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}


public class UpdateBlogCommandValidator : AbstractValidator<UpdateBlogCommand>
{
    public UpdateBlogCommandValidator()
    {
        RuleFor(b => b.BlogId != Guid.Empty);
        RuleFor(p => p.Title).NotEmpty();
        RuleFor(p => p.Description).NotEmpty();
    }
}